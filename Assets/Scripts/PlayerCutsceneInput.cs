using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCutsceneInput : MonoBehaviour
{
    public event EventHandler OnPlayerConfirm;

    private PlayerCutsceneInputActions playerCutsceneInput;
        
    private void Awake()
    {
        playerCutsceneInput = new PlayerCutsceneInputActions();
    }

    private void OnEnable()
    {
        playerCutsceneInput.Player.Enable();
        playerCutsceneInput.Player.Confirmation.performed += Confirmation_performed;
    }
    
    private void OnDisable()
    {
        playerCutsceneInput.Player.Confirmation.performed -= Confirmation_performed;
        playerCutsceneInput.Player.Disable();
    }

    private void Confirmation_performed(InputAction.CallbackContext obj)
    {
        OnPlayerConfirm?.Invoke(this, EventArgs.Empty);
    }
}