using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMapInput playerMapInput;

    private void Awake()
    {
        playerMapInput = new PlayerMapInput();
        playerMapInput.Player.Enable();
    }

    public Vector2 GetMoveDirections()
    {
        Vector2 inputDirections = playerMapInput.Player.Move.ReadValue<Vector2>();

        return inputDirections;
    }
}
