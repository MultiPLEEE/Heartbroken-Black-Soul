using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    
    [SerializeField] private PlayerMapInput playerMapInput;
    [SerializeField] private PlayerCutsceneInput playerCutsceneInput;
    
    public enum InputMode
    {
        Map,
        Cutscene,
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
                playerCutsceneInput.enabled = false;
                break;

            case InputMode.Cutscene:
                playerMapInput.enabled = false;
                playerCutsceneInput.enabled = true;
                break;
        }
    }
}
