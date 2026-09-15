using System;
using UnityEngine;

[Serializable]
public class SetMusicInfo
{
    public AudioClip clip;
}

public class SetMusic : IEventStep
{
    private SetMusicInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.setMusic;
        _context =  eventContext;
        _onComplete = onComplete;
        
        _context.soundManager.SetMusic(_info.clip);
        
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