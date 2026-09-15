using System;
using UnityEngine;


[Serializable]
public class GiveItemInfo
{
    public ConsumableItemSO itemToGive;
    public int amount = 1;
}

public class GiveItem : IEventStep
{
    private GiveItemInfo _info;
    private EventContext _context;
    private Action _onComplete;

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.giveItem;
        _context = eventContext;
        _onComplete = onComplete;
        
        Inventory playerInventory = _context.player.GetPlayerInventory();
        playerInventory.AddItem(_info.itemToGive, _info.amount);
        
        End();
    }
    
    public void Update()
    {
        
    }

    private void End()
    {
        _onComplete?.Invoke();
    }
}
