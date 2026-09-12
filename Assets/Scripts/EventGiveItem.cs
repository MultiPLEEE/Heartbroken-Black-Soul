using System;
using UnityEngine;


[Serializable]
public class GiveItemInfo
{
    public ConsumableItemSO itemToGive;
    public int amount = 1;
}

public class EventGiveItem : IEventStep
{
    public bool IsRunning { get; private set; }

    public void Execute(Step step, EventContext context, Action onComplete)
    {
        GiveItemInfo info = step.giveItem;
        
        if (info.itemToGive == null)
        {
            Debug.LogWarning("На объекте не назначен предмет!");
            return;
        }
        
        Inventory playerInventory = context.player.GetPlayerInventory();
        playerInventory.AddItem(info.itemToGive, info.amount);
    }
    
    public void Skip(Step step, EventContext context, Action onComplete)
    {
        
    }

    public void End(Step step, EventContext context, Action onComplete)
    {
        onComplete?.Invoke();
    }
}
