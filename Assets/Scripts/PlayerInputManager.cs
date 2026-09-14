using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    
    [SerializeField] private PlayerMapInput playerMapInput;
    [SerializeField] private PlayerEventInput playerEventInput;
    
    public enum InputMode
    {
        Map,
        Event,
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetInputMode(InputMode.Map);
    }

    public void SetInputMode(InputMode inputMode)
    {
        switch (inputMode)
        {
            case InputMode.Map:
                playerMapInput.enabled = true;
                playerEventInput.enabled = false;
                break;
            case InputMode.Event:
                playerMapInput.enabled = false;
                playerEventInput.enabled = true;
                break;
        }
    }
}
