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
    public bool IsRunning { get; private set; }
    
    private GiveItemInfo _info;
    private EventContext _context;
    private Action _onComplete;

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.giveItem;
        _context = eventContext;
        _onComplete = onComplete;
        
        if (_info.itemToGive == null)
        {
            Debug.LogWarning("На объекте не назначен предмет!");
            return;
        }
        
        Inventory playerInventory = eventContext.player.GetPlayerInventory();
        playerInventory.AddItem(_info.itemToGive, _info.amount);

        End();
    }
    
    public void Update()
    {
        
    }
    
    public void Skip()
    {
        
    }

    public void End()
    {
        _onComplete?.Invoke();
    }
}
