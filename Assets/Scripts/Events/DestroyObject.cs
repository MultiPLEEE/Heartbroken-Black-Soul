using System;
using UnityEngine;

[Serializable]
public class DestroyObjectInfo
{
    public GameObject objectToDestroy;
}

public class DestroyObject : IEventStep
{
    private DestroyObjectInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.destroyObject;
        _context = eventContext;
        _onComplete = onComplete;

        UnityEngine.Object.Destroy(_info.objectToDestroy);
        
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