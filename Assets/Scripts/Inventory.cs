using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxAmount = 99;
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    public bool AddItem(ConsumableItemSO item, int amount = 1)
    {
        InventorySlot existingSlot = slots.Find(slot => slot.item == item);
        
        if (existingSlot != null)
        {
            if (existingSlot.amount + amount > maxAmount) return false;
            existingSlot.amount += amount;
        }
        else
        {
            slots.Add(new InventorySlot(item, amount));
        }
        
        return true;
    }

    public void RemoveItem(ConsumableItemSO item, int amount = 1)
    {
        InventorySlot existingSlot = slots.Find(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.amount -= amount;

            if (existingSlot.amount == 0)
            {
                slots.Remove(existingSlot);
            }
        }
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return slots;
    }
}
