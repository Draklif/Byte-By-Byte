using UnityEngine;

public struct Item
{
    int itemId;
    string itemName;
    string itemDesc;
    int quantity;
    int price;
    int rarity;
    int[] statValues;
    Day[] availableDays;
    Stat[] statNames;

    public Item(int itemId, string itemName, string itemDesc, int quantity, int[] statValues, int price, int rarity, Day[] availableDays, Stat[] statNames)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.itemDesc = itemDesc;
        this.quantity = quantity;
        this.price = price;
        this.rarity = rarity;
        this.statValues = statValues;
        this.availableDays = availableDays;
        this.statNames = statNames;
    }

    public Item(ItemScriptableObject item)
    {
        this.itemId = item.itemId;
        this.itemName = item.itemName;
        this.itemDesc = item.itemDesc;
        this.quantity = item.quantity;
        this.price = item.price;
        this.rarity = item.rarity;
        this.statValues = item.statValues;
        this.availableDays = item.availableDays;
        this.statNames = item.statNames;
    }

    public bool RemoveUse(int value)
    {
        quantity -= value;
        if (quantity <= 0) return true;
        return false;
    }

    public void AddUse(int value)
    {
        quantity += value;
    }
    public int GetItemId()
    {
        return itemId;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public bool IsAvailable(Day day) 
    {
        bool isAvailable = false;
        foreach (Day actualDay in availableDays)
        {
            if (actualDay == day)
            {
                isAvailable = true;
            }
        }
        return isAvailable;
    }

    public bool HitRarity()
    {
        int randomChance = Random.Range(0, 10);
        return randomChance <= rarity;
    }
}