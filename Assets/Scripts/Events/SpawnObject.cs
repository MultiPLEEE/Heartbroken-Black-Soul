using System;
using UnityEngine;

[Serializable]
public class SpawnObjectInfo
{
    public GameObject objectToSpawn;
    public Vector2 spawnPosition;
}

public class SpawnObject : IEventStep
{
    private SpawnObjectInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.spawnObject;
        _context = eventContext;
        _onComplete = onComplete;

        GameObject.Instantiate(_info.objectToSpawn, _info.spawnPosition,  Quaternion.identity, _context.eventObjectParent.transform);
        
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