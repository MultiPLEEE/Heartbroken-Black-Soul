using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMapInput : MonoBehaviour
{
    public event EventHandler OnPlayerInteract;
    
    private PlayerMapInputActions playerMapInput;

    private void Awake()
    {
        playerMapInput = new PlayerMapInputActions();
    }
    
    private void OnEnable()
    {
        playerMapInput.Player.Enable();
        playerMapInput.Player.Interact.performed += Interact_performed;
    }
    
    private void OnDisable()
    {
        playerMapInput.Player.Interact.performed -= Interact_performed;
        playerMapInput.Player.Disable();
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
