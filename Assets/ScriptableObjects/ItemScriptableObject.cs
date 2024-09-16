using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public int itemId;
    public string itemName;
    public string itemDesc;
    public int itemUses;
    public int quantity;
    public int price;
    public int rarity;
    public int[] statValues;
    public Day[] availableDays;
    public Stat[] statNames;
}