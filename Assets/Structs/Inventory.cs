using System.Collections;

public struct Inventory
{
    ArrayList items;

    public Inventory(ArrayList items)
    {
        this.items = items;
    }

    public Item GetItemByIndex(int index)
    {
        return (Item)items[index];
    }

    public Item? GetItemById(int id)
    {
        foreach (Item item in items)
        {
            if (item.GetItemId() == id)
            {
                return item;
            }
        }
        return null;
    }

    public Item[] GetItems()
    {
        Item[] itemsTransformed = new Item[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            itemsTransformed[i] = (Item)items[i];
        }
        return itemsTransformed;
    }

    public void AddItems(Item[] newItems)
    {
        foreach (Item newItem in newItems)
        {
            bool hasFoundItem = false;
            foreach (Item item in items)
            {
                if (newItem.Equals(item))
                {
                    item.IncreaseQuantity(newItem.GetQuantity());
                    hasFoundItem = true;
                    break;
                }
            }
            if (!hasFoundItem) items.Add(newItem);
        }
    }

    public void AddItem(Item item)
    {
        bool hasFoundItem = false;
        for (int i = 0; i < items.Count; i++)
        {
            Item actualItem = (Item)items[i];
            if (actualItem.GetItemId().Equals(item.GetItemId()))
            {
                actualItem.IncreaseQuantity(1);
                items[i] = actualItem;
                hasFoundItem = true;
                break;
            }
        }

        if (!hasFoundItem) items.Add(item);
    }

    public void RemoveItem(Item item) {
        bool hasFoundItem = false;
        for (int i = 0; i < items.Count; i++)
        {
            Item actualItem = (Item)items[i];
            if (actualItem.Equals(item))
            {
                actualItem.DecreaseQuantity(1);
                if (actualItem.GetQuantity() <= 0)
                {
                    items.RemoveAt(i);
                }
                else
                {
                    items[i] = actualItem;
                }
                hasFoundItem = true;
                break;
            }
        }

        if (!hasFoundItem) items.Remove(item);
    }

    public void ClearItems()
    {
        items.Clear();
    }

    public int GetSize()
    {
        return items.Count;
    }
}