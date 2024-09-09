using System.Collections;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] int maxItemsInShop = 5;

    ScriptableObjects SO;
    WorldManager world;
    Player player;
    HUDManager hudManager;
    Inventory shopInventory;
    Item[] possibleShopItems;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
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
    }

    void GenerateShop()
    {
        shopInventory.ClearItems();
        ArrayList shopItemIds = new ArrayList();

        while (shopInventory.GetSize() < maxItemsInShop) 
        {
            bool reroll = false;
            Item randomItem;
            do
            {
                int randomItemIndex = Random.Range(0, possibleShopItems.Length);
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

        foreach (string idString in ids)
        {
            int id = int.Parse(idString);
            Item? itemToAdd = FindItemById(id);
            if (itemToAdd != null)
            {
                shopInventory.AddItem(itemToAdd.Value); // Añadir el ítem a la tienda
            }
        }
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

    void Buy(Item item)
    {
        if (CanBuy(item))
        {
            shopInventory.RemoveItem(item);
            player.AddItem(item);
        }
    }

    bool CanBuy(Item item)
    {
        return player.GetMoney() >= item.GetPrice();
    }

    void SaveShopIds(ArrayList shopItemIds)
    {
        string shopIds = string.Join(",", shopItemIds.ToArray());
        PlayerPrefs.SetString("ShopItemIds", shopIds);
        PlayerPrefs.Save();
    }

    void LoadShopFromSavedIds()
    {
        if (PlayerPrefs.HasKey("ShopItemIds"))
        {
            string savedIds = PlayerPrefs.GetString("ShopItemIds");
            string[] ids = savedIds.Split(',');

            GenerateShopByIds(ids);
        }
        else
        {
            GenerateShop();
        }
    }
}
