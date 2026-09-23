using System;

[Serializable]
public class InventorySlot
{
    public ConsumableItemSo item;
    public int amount;
    
    public InventorySlot(ConsumableItemSo item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }
}