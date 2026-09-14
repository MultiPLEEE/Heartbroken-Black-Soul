using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEventInput : MonoBehaviour
{
    public event EventHandler OnPlayerConfirm;
    public event EventHandler OnPlayerPrevious;
    public event EventHandler OnPlayerUp;
    public event EventHandler OnPlayerDown;
    public event EventHandler OnPlayerLeft;
    public event EventHandler OnPlayerRight;

    private PlayerEventInputActions _playerEventInput;
        
    private void Awake()
    {
        _playerEventInput = new PlayerEventInputActions();
    }

    private void OnEnable()
    {
        _playerEventInput.Player.Enable();
        _playerEventInput.Player.Confirmation.performed += Confirmation_performed;
        _playerEventInput.Player.Previous.performed += Previous_performed;
        _playerEventInput.Player.Up.performed += Up_performed;
        _playerEventInput.Player.Down.performed += Down_performed;
        _playerEventInput.Player.Left.performed += Left_performed;
        _playerEventInput.Player.Right.performed += Right_performed;
        
    }
    
    private void OnDisable()
    {
        _playerEventInput.Player.Confirmation.performed -= Confirmation_performed;
        _playerEventInput.Player.Previous.performed -= Previous_performed;
        _playerEventInput.Player.Up.performed -= Up_performed;
        _playerEventInput.Player.Down.performed -= Down_performed;
        _playerEventInput.Player.Left.performed -= Left_performed;
        _playerEventInput.Player.Right.performed -= Right_performed;
        _playerEventInput.Player.Disable();
    }

    private void Confirmation_performed(InputAction.CallbackContext obj)
    {
        OnPlayerConfirm?.Invoke(this, EventArgs.Empty);
    }
    private void Previous_performed(InputAction.CallbackContext obj)
    {
        OnPlayerPrevious?.Invoke(this, EventArgs.Empty);
    }
    
    private void Up_performed(InputAction.CallbackContext obj)
    {
        OnPlayerUp?.Invoke(this, EventArgs.Empty);
    }
    
    private void Down_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDown?.Invoke(this, EventArgs.Empty);
    }
    
    private void Left_performed(InputAction.CallbackContext obj)
    {
        OnPlayerLeft?.Invoke(this, EventArgs.Empty);
    }
    
    private void Right_performed(InputAction.CallbackContext obj)
    {
        OnPlayerRight?.Invoke(this, EventArgs.Empty);
    }
}