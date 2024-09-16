using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HUDManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI RemainingTimer;
    [SerializeField] public TextMeshProUGUI ActualTimer;
    [SerializeField] public TextMeshProUGUI ActualTimeOfDay;
    [SerializeField] public TextMeshProUGUI ActualDayOfWeek;
    [SerializeField] public TextMeshProUGUI HappinessValue;
    [SerializeField] public TextMeshProUGUI HungerValue;
    [SerializeField] public TextMeshProUGUI StressValue;
    [SerializeField] public TextMeshProUGUI WeightValue;
    [SerializeField] public TextMeshProUGUI KnowledgeValue;
    [SerializeField] public TextMeshProUGUI MoneyValue;

    [SerializeField] public GameObject activityUI;

    [SerializeField] public GameObject debugPanel;
    [SerializeField] public GameObject shopPanel;
    [SerializeField] public Transform[] shopItemContainers;
    [SerializeField] public GameObject shopItemPrefab;
    [SerializeField] public GameObject inventoryPanel;
    [SerializeField] public Transform[] inventoryItemContainers;
    [SerializeField] public GameObject inventoryItemPrefab;

    [SerializeField] public Button SkipButton;
    [SerializeField] public TMP_Dropdown TimeSelect;
    [SerializeField] public TMP_Dropdown ActivitySelect;

    [SerializeField] GameObject alertPanel;
    [SerializeField] TMP_Text alertTitle;
    [SerializeField] TMP_Text alertDescription;
    [SerializeField] Button alertYesButton;
    [SerializeField] Button alertNoButton;

    private WorldManager worldManager;
    private ActivityManager activityManager;
    private ShopManager shopManager;
    private Player player;
    private ScriptableObjects SO;

    public List<PlannedActivity> activityQueue = new List<PlannedActivity>();

    void Start()
    {
        // Inyectar dependencias desde el WorldManager, ActivityManager, y ShopManager
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        activityManager = GameObject.FindGameObjectWithTag("Activity").GetComponent<ActivityManager>();
        shopManager = GameObject.FindGameObjectWithTag("Shop").GetComponent<ShopManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();

        // Suscribir a eventos
        worldManager.OnNewWeek += UpdateDayOfWeekUI;
        worldManager.OnNewTimeOfDay += UpdateTimeOfDayUI;
        activityManager.OnActivityCompleted += UpdateActivityUI;
        activityManager.OnActivityStarted += UpdateActivityUIVisibility;
        activityManager.OnTimerUpdated += UpdateRemainingTimeUI;
        shopManager.OnShopUpdated += UpdateShopUI;
        player.OnInventoryUpdated += UpdateInventoryUI;

        // Inicializar el UI basado en los estados actuales
        InitializeUI();
    }

    void Update()
    {
        UpdateTimeUI();
    }

    void UpdateTimeUI()
    {
        ActualTimer.text = string.Format("{0:D2}:{1:D2}", worldManager.GetActualDate().Hour, worldManager.GetActualDate().Minute);
    }

    void UpdateTimeOfDayUI()
    {
        ActualTimeOfDay.text = worldManager.GetActualTime().ToString();
    }

    void UpdateDayOfWeekUI()
    {
        ActualDayOfWeek.text = worldManager.GetActualDay().ToString();
    }

    void UpdateActivityUIVisibility()
    {
        activityUI.SetActive(true);
    }

    void UpdateActivityUI()
    {
        RemainingTimer.text = "00:00:00";
        activityUI.SetActive(false);
    }

    void UpdateRemainingTimeUI(TimeSpan remainingTime)
    {
        RemainingTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
    }

    void UpdateShopUI()
    {
        Inventory shopInventory = shopManager.GetShopInventory();
        Item[] shopItems = shopInventory.GetItems();

        int numberOfItemsToShow = Mathf.Min(shopItems.Length, shopItemContainers.Length);

        for (int i = 0; i < shopItemContainers.Length; i++)
        {
            foreach (Transform child in shopItemContainers[i])
            {
                Destroy(child.gameObject);
            }

            if (i < numberOfItemsToShow)
            {
                GameObject newItemUI = Instantiate(shopItemPrefab, shopItemContainers[i]);

                TMP_Text itemName = newItemUI.transform.Find("ItemName").GetComponent<TMP_Text>();
                TMP_Text itemPrice = newItemUI.transform.Find("ItemPrice").GetComponent<TMP_Text>();
                TMP_Text itemQuantity = newItemUI.transform.Find("ItemQuantity").GetComponent<TMP_Text>();
                Button buyButton = newItemUI.transform.Find("BuyButton").GetComponent<Button>();

                itemName.text = shopItems[i].GetItemName();
                itemPrice.text = shopItems[i].GetPrice().ToString();
                itemQuantity.text = shopItems[i].GetQuantity().ToString();

                int index = i;
                buyButton.onClick.AddListener(() => shopManager.Buy(shopItems[index]));
            }
        }
    }

    void UpdateInventoryUI()
    {
        Inventory playerInventory = player.GetPlayerStats().GetInventory();

        Item[] playerItems = playerInventory.GetItems();

        int numberOfItemsToShow = Mathf.Min(playerItems.Length, inventoryItemContainers.Length);

        for (int i = 0; i < inventoryItemContainers.Length; i++)
        {
            foreach (Transform child in inventoryItemContainers[i])
            {
                Destroy(child.gameObject);
            }

            if (i < numberOfItemsToShow)
            {
                GameObject newItemUI = Instantiate(inventoryItemPrefab, inventoryItemContainers[i]);

                TMP_Text itemName = newItemUI.transform.Find("ItemName").GetComponent<TMP_Text>();
                TMP_Text itemQuantity = newItemUI.transform.Find("ItemQuantity").GetComponent<TMP_Text>();
                Button useButton = newItemUI.transform.Find("UseButton").GetComponent<Button>();

                itemName.text = playerItems[i].GetItemName();
                itemQuantity.text = playerItems[i].GetQuantity().ToString();

                int index = i;
                useButton.onClick.AddListener(() => player.UseItem(playerItems[index]));
            }
        }
    }

    void InitializeUI()
    {
        UpdateTimeOfDayUI();
        UpdateDayOfWeekUI();
        UpdateShopUI();
        UpdateInventoryUI();
        LoadPlanner();
    }

    public void LoadPlanner()
    {
        int hour = worldManager.GetActualDate().Hour;

        TimeSelect.options.Clear();
        for (int i = hour + 1; i < 24; i++)
        {
            TimeSelect.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        TimeSelect.RefreshShownValue();
    }

    public void AddActivityToQueue()
    {
        int selectedHour = int.Parse(TimeSelect.options[TimeSelect.value].text);

        int selectedActivityIndex = ActivitySelect.value;
        string selectedActivityName = ActivitySelect.options[selectedActivityIndex].text;

        ActivityScriptableObject selectedActivity = null;
        foreach (ActivityScriptableObject activity in SO.scriptableActivities)
        {
            if (activity.activityName == selectedActivityName)
            {
                selectedActivity = activity;
                break;
            }
        }

        if (selectedActivity != null)
        {
            PlannedActivity newPlannedActivity = new PlannedActivity(new Activity(selectedActivity), selectedHour);
            activityQueue.Add(newPlannedActivity);

            TimeSelect.options.RemoveAt(TimeSelect.value);
            TimeSelect.RefreshShownValue();
        }
    }

    public void RemoveActivityFromQueue(int activityIndex)
    {
        if (activityIndex < 0 || activityIndex >= activityQueue.Count)
        {
            return;
        }

        PlannedActivity activityToRemove = activityQueue[activityIndex];

        activityQueue.RemoveAt(activityIndex);

        int removedHour = activityToRemove.hour;
        TimeSelect.options.Add(new TMP_Dropdown.OptionData(removedHour.ToString()));
        TimeSelect.options.Sort((a, b) => int.Parse(a.text).CompareTo(int.Parse(b.text)));
        TimeSelect.RefreshShownValue();
    }

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ToggleShop(bool toggle)
    {
        shopPanel.SetActive(toggle);
    }

    public void ToggleInventory(bool toggle)
    {
        inventoryPanel.SetActive(toggle);
    }

    public void ToggleDebug(bool toggle)
    {
        debugPanel.SetActive(toggle);
    }

    public void ShowAlert(string title, string description, Action onYes, Action onNo)
    {
        alertTitle.text = title;
        alertDescription.text = description;

        alertYesButton.onClick.RemoveAllListeners();
        alertNoButton.onClick.RemoveAllListeners();

        alertYesButton.onClick.AddListener(() => {
            onYes?.Invoke();
            alertPanel.SetActive(false);
        });

        alertNoButton.onClick.AddListener(() => {
            onNo?.Invoke();
            alertPanel.SetActive(false);
        });

        alertPanel.SetActive(true);
    }

}