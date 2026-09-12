using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEventInput : MonoBehaviour
{
    public event EventHandler OnPlayerConfirm;

    private PlayerEventInputActions _playerEventInput;
        
    private void Awake()
    {
        _playerEventInput = new PlayerEventInputActions();
    }

    private void OnEnable()
    {
        _playerEventInput.Player.Enable();
        _playerEventInput.Player.Confirmation.performed += Confirmation_performed;
    }
    
    private void OnDisable()
    {
        _playerEventInput.Player.Confirmation.performed -= Confirmation_performed;
        _playerEventInput.Player.Disable();
    }

    private void Confirmation_performed(InputAction.CallbackContext obj)
    {
        OnPlayerConfirm?.Invoke(this, EventArgs.Empty);
    }
}