using System;

[Serializable]
public class InventorySlot
{
    public ConsumableItemSO item;
    public int amount;
    
    public InventorySlot(ConsumableItemSO item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }
}
