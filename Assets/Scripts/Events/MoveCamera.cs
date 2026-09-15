using System;
using UnityEngine;

[Serializable]
public class MoveCameraInfo
{
    public Vector2 targetPosition;
}

public class MoveCamera : IEventStep
{
    private float cameraSpeed = 4;
    
    private MoveCameraInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.moveCamera;
        _context =  eventContext;
        _onComplete = onComplete;
    }

    public void Update()
    {
        _context.cameraTarget.transform.localPosition = Vector3.MoveTowards(_context.cameraTarget.transform.localPosition, _info.targetPosition,  cameraSpeed * Time.deltaTime);
        
        if (_context.cameraTarget.transform.localPosition == (Vector3)_info.targetPosition)
        {
            _context.cameraTarget.transform.localPosition = _info.targetPosition;
            End();
        }
    }
    
    private void End()
    {
        _onComplete?.Invoke();
    }
}