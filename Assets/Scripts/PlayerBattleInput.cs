using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleInput : MonoBehaviour
{
    public event EventHandler OnPlayerConfirm;
    public event EventHandler OnPlayerPrevious;
    public event EventHandler OnPlayerUp;
    public event EventHandler OnPlayerDown;
    public event EventHandler OnPlayerLeft;
    public event EventHandler OnPlayerRight;

    private PlayerBattleInputActions _playerBattleInput;
        
    private void Awake()
    {
        _playerBattleInput = new PlayerBattleInputActions();
    }

    private void OnEnable()
    {
        _playerBattleInput.Player.Enable();
        _playerBattleInput.Player.Confirmation.performed += Confirmation_performed;
        _playerBattleInput.Player.Previous.performed += Previous_performed;
        _playerBattleInput.Player.Up.performed += Up_performed;
        _playerBattleInput.Player.Down.performed += Down_performed;
        _playerBattleInput.Player.Left.performed += Left_performed;
        _playerBattleInput.Player.Right.performed += Right_performed;
        
    }
    
    private void OnDisable()
    {
        _playerBattleInput.Player.Confirmation.performed -= Confirmation_performed;
        _playerBattleInput.Player.Previous.performed -= Previous_performed;
        _playerBattleInput.Player.Up.performed -= Up_performed;
        _playerBattleInput.Player.Down.performed -= Down_performed;
        _playerBattleInput.Player.Left.performed -= Left_performed;
        _playerBattleInput.Player.Right.performed -= Right_performed;
        _playerBattleInput.Player.Disable();
    }

    private void Confirmation_performed(InputAction.CallbackContext obj)
    {
        OnPlayerConfirm?.Invoke(this, EventArgs.Empty);
    }
    private void Previous_performed(InputAction.CallbackContext obj)
    {
        OnPlayerConfirm?.Invoke(this, EventArgs.Empty);
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