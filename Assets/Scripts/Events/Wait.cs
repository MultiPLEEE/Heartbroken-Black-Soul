using System;
using UnityEngine;

[Serializable]
public class WaitInfo
{
    public float duration;
}

public class Wait : IEventStep
{
    private WaitInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    private float _timer;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.wait;
        _context = eventContext;
        _onComplete = onComplete;
        
        _timer = _info.duration;
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            End();
        }
    }
    
    private void End()
    {
        _onComplete?.Invoke();
    }
}