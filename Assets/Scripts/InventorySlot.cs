using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ScriptableItemSO item;
    public int amount;
    
    public InventorySlot(ScriptableItemSO item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }
}
