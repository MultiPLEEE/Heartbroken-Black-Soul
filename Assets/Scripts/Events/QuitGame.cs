using System;
using UnityEngine;

[Serializable]
public class quitGameInfo
{

}

public class QuitGame : IEventStep
{
    private quitGameInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.quitGame;
        _context =  eventContext;
        _onComplete = onComplete;
        
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
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