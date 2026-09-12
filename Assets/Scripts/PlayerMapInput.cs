using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMapInput : MonoBehaviour
{
    public event EventHandler OnPlayerInteract;
    
    private PlayerMapInputActions _playerMapInput;

    private void Awake()
    {
        _playerMapInput = new PlayerMapInputActions();
    }
    
    private void OnEnable()
    {
        _playerMapInput.Player.Enable();
        _playerMapInput.Player.Interact.performed += Interact_performed;
    }
    
    private void OnDisable()
    {
        _playerMapInput.Player.Interact.performed -= Interact_performed;
        _playerMapInput.Player.Disable();
    }
    
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnPlayerInteract?.Invoke(this, EventArgs.Empty);
    }
    
    public Vector2 GetPlayerMoveVector()
    {
        Vector2 inputDirections = _playerMapInput.Player.Move.ReadValue<Vector2>();
        return inputDirections;
    }
    
    public bool IsSprintPressed()
    {
        return _playerMapInput.Player.Sprint.IsPressed();
    }

}
