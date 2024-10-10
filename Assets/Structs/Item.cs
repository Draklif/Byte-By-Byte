using UnityEngine;

public struct Item
{
    int itemId;
    string itemName;
    string itemDesc;
    int itemUses;
    int quantity;
    int price;
    int rarity;
    int[] statValues;
    Day[] availableDays;
    Stat[] statNames;
    int effectId;

    public Item(int itemId, string itemName, string itemDesc, int itemUses, int quantity, int[] statValues, int price, int rarity, Day[] availableDays, Stat[] statNames, int effectId)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.itemDesc = itemDesc;
        this.itemUses = itemUses;
        this.quantity = quantity;
        this.price = price;
        this.rarity = rarity;
        this.statValues = statValues;
        this.availableDays = availableDays;
        this.statNames = statNames;
        this.effectId = effectId;
    }

    public Item(ItemScriptableObject item)
    {
        this.itemId = item.itemId;
        this.itemName = item.itemName;
        this.itemDesc = item.itemDesc;
        this.itemUses = item.itemUses;
        this.quantity = item.quantity;
        this.price = item.price;
        this.rarity = item.rarity;
        this.statValues = item.statValues;
        this.availableDays = item.availableDays;
        this.statNames = item.statNames;
        this.effectId = item.effectId;
    }

    public bool DecreaseQuantity(int value)
    {
        quantity -= value;
        if (quantity <= 0) return true;
        return false;
    }

    public void IncreaseQuantity(int value)
    {
        quantity += value;
    }

    public int GetEffectId()
    {
        return effectId;
    }

    public int GetItemId()
    {
        return itemId;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public int GetItemUses()
    {
        return itemUses;
    }

    public Stat[] GetStatNames()
    {
        return statNames;
    }
    public int[] GetStatValues()
    {
        return statValues;
    }

    public void SetQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public void SetItemUses(int itemUses)
    {
        this.itemUses = itemUses;
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