using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ScriptableСonsumableSO item;
    public int amount;
    
    public InventorySlot(ScriptableСonsumableSO item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }
}
