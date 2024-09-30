using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerStats playerStats;

    HUDManager hudManager;
    WorldManager world;
    ScriptableObjects SO;
    Dictionary<int, int> itemUsages;
    List<SpecialEffects> activeEffects;

    float baseSalary = 10, positionMultiplier = 1, knowledgeMultiplier = 0.5f;

    public event Action OnInventoryUpdated;

    void Start()
    {
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
        world = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();
        itemUsages = new Dictionary<int, int>();
        activeEffects = new List<SpecialEffects>();

        LoadPlayerStats();
        LoadInventory();
        LoadItemUsages();
        LoadSalary();
        SavePlayerStats();

        world.OnNewDay += ResetItemUses;
        world.OnNewDay += ResetSleep;
    }

    void Update()
    {
        hudManager.HappinessValue.text = playerStats.GetPlayerMeters().GetHappiness().ToString();
        hudManager.HungerValue.text = playerStats.GetPlayerMeters().GetHunger().ToString();
        hudManager.StressValue.text = playerStats.GetPlayerMeters().GetStress().ToString();
        hudManager.WeightValue.text = playerStats.GetPlayerMeters().GetWeight().ToString();
        hudManager.KnowledgeValue.text = playerStats.GetPlayerMeters().GetKnowledge().ToString();
        hudManager.MoneyValue.text = playerStats.GetMoney().ToString();
    }

    public void AddSpecialEffect(SpecialEffects effect)
    {
        activeEffects.Add(effect);
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public void ModifyPlayerStats(Stat[] activityStatNames, int[] activityStatValues)
    {
        for (int i = 0; i < activityStatNames.Length; i++)
        {
            float modifier = GetStatModifier(activityStatNames[i]);
            bool affectsIncrease = GetStatAffectsIncrease(activityStatNames[i]);
            bool affectsDecrease = GetStatAffectsDecrease(activityStatNames[i]);

            int value = activityStatValues[i];
            float modifiedValue = value;

            if (modifier != 1f)
            {
                if (affectsIncrease && value > 0)
                {
                    modifiedValue = value * modifier;
                }
                else if (affectsDecrease && value <= 0)
                {
                    modifiedValue = value * modifier;
                }
            }

            this.playerStats.ModifyStats(activityStatNames[i], (int)modifiedValue);
        }
        SavePlayerStats();
    }

    float GetStatModifier(Stat stat)
    {
        float modifier = 1f;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            SpecialEffects effect = activeEffects[i];
            if (effect.effectData.statAffected == stat)
            {
                if (effect.IsExpired())
                {
                    activeEffects.RemoveAt(i);
                }
                else
                {
                    modifier *= effect.effectData.modifier;
                }
            }
        }

        return modifier;
    }

    bool GetStatAffectsIncrease(Stat stat)
    {
        bool affectsIncrease = false;

        foreach (SpecialEffects effect in activeEffects)
        {
            if (effect.effectData.statAffected == stat)
            {
                affectsIncrease = effect.effectData.affectsIncrease;
            }
        }

        return affectsIncrease;
    }

    bool GetStatAffectsDecrease(Stat stat)
    {
        bool affectsDecrese = false;

        foreach (SpecialEffects effect in activeEffects)
        {
            if (effect.effectData.statAffected == stat)
            {
                affectsDecrese = effect.effectData.affectsDecrease;
            }
        }

        return affectsDecrese;
    }

    public void SetSalary(float baseSalary, float positionMultiplier, float knowledgeMultiplier)
    {
        this.baseSalary = baseSalary;
        this.positionMultiplier = positionMultiplier;
        this.knowledgeMultiplier = knowledgeMultiplier;
        SaveSalary();
    }

    public float GetBaseSalary() { return baseSalary; }
    public float GetPositionMultiplier() { return positionMultiplier; }
    public float GetKnowledgeMultiplier() { return knowledgeMultiplier; }

    public void ModifyMoney(int value)
    {
        this.playerStats.ModifyMoney(value);
        SavePlayerStats();
    }

    public void ModifyMoneyWork(bool halved)
    {
        float money = (baseSalary * positionMultiplier) +(playerStats.GetPlayerMeters().GetKnowledge() * knowledgeMultiplier) + UnityEngine.Random.Range(0.8f, 1.2f);

        if (halved)
        {
            this.playerStats.ModifyMoney((int)money / 2);
        }
        else
        {
            this.playerStats.ModifyMoney((int)money);
        }
        SavePlayerStats();
    }

    public bool CanSleep()
    {
        string sleepString = PlayerPrefs.GetString("Player_SleepTime", "");

        DateTime lastSleepTime;
        if (!string.IsNullOrEmpty(sleepString))
        {
            lastSleepTime = DateTime.Parse(sleepString);
        }
        else
        {
            lastSleepTime = DateTime.MinValue;
        }

        DateTime currentTime = DateTime.Now;

        // Verificar si han pasado al menos 2 horas desde el último sueño
        TimeSpan timeSinceLastSleep = currentTime - lastSleepTime;
        if (timeSinceLastSleep.TotalHours < 2)
        {
            return false;
        }

        // Verificar si ha dormido más de 6 horas en el día
        int sleepHoursToday = PlayerPrefs.GetInt("Player_SleepHoursToday", 0);
        if (sleepHoursToday >= 6)
        {
            return false;
        }

        // Verificar si no tiene hambre
        if (playerStats.GetPlayerMeters().GetHunger() < 0)
        {
            return false;
        }

        PlayerPrefs.SetString("Player_SleepTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
        return true;
    }

    void ResetSleep()
    {
        PlayerPrefs.SetInt("Player_SleepHoursToday", 0);
        PlayerPrefs.Save();
    }

    public void Sleep(bool interrupted)
    {
        string sleepString = PlayerPrefs.GetString("Player_SleepTime", "");

        DateTime lastSleepTime;
        if (!string.IsNullOrEmpty(sleepString))
        {
            lastSleepTime = DateTime.Parse(sleepString);
        }
        else
        {
            lastSleepTime = DateTime.MinValue;
        }

        DateTime currentTime = DateTime.Now;
        TimeSpan timeSlept = currentTime - lastSleepTime;

        int sleepHoursToday = PlayerPrefs.GetInt("Player_SleepHoursToday", 0);
        sleepHoursToday += (int)timeSlept.TotalHours;
        PlayerPrefs.SetInt("Player_SleepHoursToday", sleepHoursToday);
        PlayerPrefs.Save();

        if (timeSlept.TotalHours >= 4)
        {
            ModifyPlayerStats(new Stat[] { Stat.Hunger }, new int[] { -10 });
        }

        if (interrupted)
        {
            return;
        }

        if (playerStats.GetPlayerMeters().GetHunger() > 30)
        {
            ModifyPlayerStats(new Stat[] { Stat.Happiness }, new int[] { 10 });
        }

        ModifyPlayerStats(new Stat[] { Stat.Happiness, Stat.Stress, Stat.Hunger }, new int[] { 10, -30, -10 });
    }

    public void AddItem(Item item)
    {
        this.playerStats.GetInventory().AddItem(item);
        SaveInventory();
        OnInventoryUpdated?.Invoke();
    }

    public void RemoveItem(Item item)
    {
        this.playerStats.GetInventory().RemoveItem(item);
        SaveInventory();
        OnInventoryUpdated?.Invoke();
    }

    public void UseItem(Item item)
    {
        int itemId = item.GetItemId();
        if (itemUsages.ContainsKey(itemId) && itemUsages[itemId] >= item.GetItemUses())
        {
            Debug.Log("No more uses remaining for this item today.");
            return;
        }

        if (itemUsages.ContainsKey(itemId))
        {
            itemUsages[itemId]++;
        }
        else
        {
            itemUsages[itemId] = 1;
        }

        ModifyPlayerStats(item.GetStatNames(), item.GetStatValues());
        RemoveItem(item);
        SaveItemUsages();
    }

    public int GetMoney()
    {
        return this.playerStats.GetMoney();
    }

    void SaveInventory()
    {
        Inventory inventory = playerStats.GetInventory();
        Item[] items = inventory.GetItems();

        PlayerPrefs.DeleteKey("Player_Inventory");

        List<string> inventoryData = new List<string>();
        foreach (Item item in items)
        {
            string itemData = item.GetItemId() + ":" + item.GetQuantity() + ":" + item.GetItemUses();
            inventoryData.Add(itemData);
        }

        string savedData = string.Join(",", inventoryData.ToArray());
        PlayerPrefs.SetString("Player_Inventory", savedData);
        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Player_Inventory"))
        {
            string savedData = PlayerPrefs.GetString("Player_Inventory");

            string[] itemStrings = savedData.Split(',');

            foreach (string itemString in itemStrings)
            {
                string[] itemData = itemString.Split(':');

                if (itemData.Length != 1)
                {
                    int itemId = int.Parse(itemData[0]);
                    int itemQuantity = int.Parse(itemData[1]);

                    Item? item = FindItemById(itemId);
                    if (item != null)
                    {
                        Item newItem = item.Value;
                        newItem.SetQuantity(itemQuantity);

                        playerStats.GetInventory().AddItem(newItem);
                    }
                }
            }
        }
    }

    public Item? FindItemById(int id)
    {
        foreach (ItemScriptableObject item in SO.scriptableShopItems)
        {
            if (item.itemId == id)
            {
                return new Item(item);
            }
        }
        return null;
    }

    void ResetItemUses()
    {
        itemUsages.Clear();

        Inventory inventory = playerStats.GetInventory();
        Item[] items = inventory.GetItems();
        foreach (Item item in items)
        {
            PlayerPrefs.DeleteKey("Item_" + item.GetItemId());
        }
        PlayerPrefs.Save();
    }

    void LoadItemUsages()
    {
        Inventory inventory = playerStats.GetInventory();
        Item[] items = inventory.GetItems();
        foreach (Item item in items)
        {
            int uses = PlayerPrefs.GetInt("Item_" + item.GetItemId(), 0);
            itemUsages[item.GetItemId()] = uses;
        }
    }

    void SaveItemUsages()
    {
        foreach (var usage in itemUsages)
        {
            PlayerPrefs.SetInt("Item_" + usage.Key, usage.Value);
        }
        PlayerPrefs.Save();
    }

    void SavePlayerStats()
    {
        PlayerMeters meters = playerStats.GetPlayerMeters();
        PlayerPrefs.SetInt("Player_Happiness", meters.GetHappiness());
        PlayerPrefs.SetInt("Player_Hunger", meters.GetHunger());
        PlayerPrefs.SetInt("Player_Stress", meters.GetStress());
        PlayerPrefs.SetInt("Player_Weight", meters.GetWeight());
        PlayerPrefs.SetInt("Player_Knowledge", meters.GetKnowledge());
        PlayerPrefs.SetInt("Player_Money", playerStats.GetMoney());
        PlayerPrefs.SetString("Player_Stage", playerStats.GetStage().ToString());
        PlayerPrefs.Save();
    }

    void LoadPlayerStats()
    {
        PlayerMeters meters = new PlayerMeters(
            PlayerPrefs.GetInt("Player_Happiness", 30),
            PlayerPrefs.GetInt("Player_Hunger", 10),
            PlayerPrefs.GetInt("Player_Stress", 30),
            PlayerPrefs.GetInt("Player_Weight", 50),
            PlayerPrefs.GetInt("Player_Knowledge", 0)
        );

        int money = PlayerPrefs.GetInt("Player_Money", 0);
        Stage stage = (Stage)Enum.Parse(typeof(Stage), PlayerPrefs.GetString("Player_Stage", Stage.Student.ToString()));

        playerStats = new PlayerStats(meters, new Inventory(new ArrayList()), stage, money);
    }

    void SaveSalary()
    {
        PlayerPrefs.SetFloat("Player_BaseSalary", baseSalary);
        PlayerPrefs.SetFloat("Player_PositionMult", positionMultiplier);
        PlayerPrefs.SetFloat("Player_KnowledgeMult", knowledgeMultiplier);
        PlayerPrefs.Save();
    }

    void LoadSalary()
    {
        baseSalary = PlayerPrefs.GetFloat("Player_BaseSalary", 10);
        positionMultiplier = PlayerPrefs.GetFloat("Player_PositionMult", 1);
        knowledgeMultiplier = PlayerPrefs.GetFloat("Player_KnowledgeMult", 0.5f);
    }

    void SaveSpecialEffects()
    {
        PlayerPrefs.SetInt("Effects_Count", activeEffects.Count);

        foreach (SpecialEffects effect in activeEffects)
        {
            PlayerPrefs.SetFloat("Effects_" + effect.effectData.effectId + "_RemainingTime", effect.GetRemainingTime());
        }

        PlayerPrefs.Save();
    }

    void LoadSpecialEffects()
    {
        activeEffects.Clear();

        int effectsCount = PlayerPrefs.GetInt("Effects_Count", 0);

        for (int i = 0; i < effectsCount; i++)
        {
            foreach (EffectScriptableObject effect in SO.scriptableEffects)
            {
                float remainingTime = PlayerPrefs.GetFloat("Effects_" + effect.effectId + "_RemainingTime", -1);

                if (remainingTime > 0)
                {
                    SpecialEffects newEffect = new SpecialEffects(effect);
                    newEffect.UpdateStartTime(DateTime.Now.AddHours(-effect.durationInHours + remainingTime));

                    if (!newEffect.IsExpired())
                    {
                        activeEffects.Add(newEffect);
                    }
                }
            }
        }
    }

    EffectScriptableObject FindEffectById(int id)
    {
        foreach (EffectScriptableObject effect in SO.scriptableEffects)
        {
            if (effect.effectId == id)
            {
                return effect;
            }
        }
        return null;
    }
}
