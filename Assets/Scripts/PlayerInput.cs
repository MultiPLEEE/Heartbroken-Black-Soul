using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public event EventHandler OnPlayerSprint;
    public event EventHandler OnPlayerInteract;
    
    private PlayerMapInput playerMapInput;

    private void Awake()
    {
        playerMapInput = new PlayerMapInput();
        playerMapInput.Player.Enable();
        
        playerMapInput.Player.Interact.performed += Interact_performed;
    }
    
    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnPlayerInteract?.Invoke(this, EventArgs.Empty);
    }
    
    public Vector2 GetPlayerMoveVector()
    {
        Vector2 inputDirections = playerMapInput.Player.Move.ReadValue<Vector2>();
    
        return inputDirections;
    }
    
    public bool IsSprintPressed()
    {
        return playerMapInput.Player.Sprint.IsPressed();
    }

}
