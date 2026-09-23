using System;
using UnityEngine;


[Serializable]
public class GiveItemInfo
{
    public ConsumableItemSo itemToGive;
    public int amount = 1;
}

public class ItemGet : IEventStep
{
    private GiveItemInfo _info;
    private EventContext _context;
    private Action _onComplete;

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.giveItem;
        _context = eventContext;
        _onComplete = onComplete;
        
        SoundManager.Instance.PlaySound(SoundManager.Instance.Database.itemGet, 0.9f);
        
        Inventory playerInventory = _context.player.GetPlayerInventory();
        playerInventory.AddItem(_info.itemToGive, _info.amount);
        
        if (ItemNotificationUI.Instance != null)
        {
            ItemNotificationUI.Instance.ShowNotification(
                _info.itemToGive.objectName, 
                _info.itemToGive.objectDescription, 
                _info.amount, 
                _info.itemToGive.sprite, 
                duration: 4f
            );
        }
        
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
