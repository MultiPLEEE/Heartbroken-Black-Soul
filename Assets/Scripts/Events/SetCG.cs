using System;
using UnityEngine;

[Serializable]
public class SetCGInfo
{
    public Sprite sprite;
}

public class SetCG : IEventStep
{
    private SetCGInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.setCG;
        _context = eventContext;
        _onComplete = onComplete;

        _context.CG.sprite = _info.sprite;
        
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