using System;
using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    ScriptableObjects SO;
    WorldManager world;
    Player player;
    Inventory shopInventory;
    Item[] possibleShopItems;

    public event Action OnShopUpdated;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        world = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();
        shopInventory = new Inventory(new ArrayList());
        possibleShopItems = new Item[SO.scriptableShopItems.Length];

        for (int i = 0; i < SO.scriptableShopItems.Length; i++)
        {
            possibleShopItems[i] = new Item(SO.scriptableShopItems[i]);
        }

        if (world.isNewDay)
        {
            OnDailyCheck();
        }
        else
        {
            LoadShopFromSavedIds();
        }
    }

    void OnDailyCheck()
    {
        GenerateShop();
        OnShopUpdated?.Invoke();
    }

    void GenerateShop()
    {
        shopInventory.ClearItems();
        ArrayList shopItemIds = new ArrayList();

        while (shopInventory.GetSize() < 3)
        {
            bool reroll = false;
            Item randomItem;
            do
            {
                int randomItemIndex = UnityEngine.Random.Range(0, possibleShopItems.Length);
                randomItem = possibleShopItems[randomItemIndex];
                reroll = randomItem.IsAvailable(world.GetActualDay()) && randomItem.HitRarity() ? false : true;
            }
            while (reroll);

            shopInventory.AddItem(randomItem);
            shopItemIds.Add(randomItem.GetItemId());
        }
        SaveShopIds(shopItemIds);
    }

    void GenerateShopByIds(string[] ids)
    {
        shopInventory.ClearItems();

        if (ids[0].Equals(""))
        {
            return;
        }

        foreach (string idString in ids)
        {
            int id = int.Parse(idString);
            Item? itemToAdd = FindItemById(id);
            if (itemToAdd != null)
            {
                shopInventory.AddItem(itemToAdd.Value);
            }
        }
        OnShopUpdated?.Invoke();
    }

    Item? FindItemById(int id)
    {
        foreach (Item item in possibleShopItems)
        {
            if (item.GetItemId() == id)
            {
                return item;
            }
        }
        return null;
    }

    public void Buy(Item item)
    {
        if (CanBuy(item))
        {
            Inventory tempInventory = shopInventory;
            tempInventory.RemoveItem(item);
            shopInventory = tempInventory;
            player.ModifyMoney(-item.GetPrice());
            player.AddItem(item);

            SaveCurrentShopState();

            OnShopUpdated?.Invoke();
        }
    }

    bool CanBuy(Item item)
    {
        return player.GetMoney() >= item.GetPrice() && player.GetPlayerStats().GetInventory().GetSize() < 3;
    }
    void SaveCurrentShopState()
    {
        ArrayList remainingItemIds = new ArrayList();

        foreach (Item item in shopInventory.GetItems())
        {
            remainingItemIds.Add(item.GetItemId());
        }

        SaveShopIds(remainingItemIds);
    }


    void SaveShopIds(ArrayList shopItemIds)
    {
        string shopIds = string.Join(",", shopItemIds.ToArray());
        PlayerPrefs.SetString("Shop_ItemIds", shopIds);
        PlayerPrefs.Save();
    }

    void LoadShopFromSavedIds()
    {
        if (PlayerPrefs.HasKey("Shop_ItemIds"))
        {
            string savedIds = PlayerPrefs.GetString("Shop_ItemIds");
            string[] ids = savedIds.Split(',');

            GenerateShopByIds(ids);
        }
        else
        {
            GenerateShop();
        }
    }

    public Inventory GetShopInventory()
    {
        return shopInventory;
    }
}
