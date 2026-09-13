using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }
    
    [SerializeField] private PlayerMapInput playerMapInput;
    [SerializeField] private PlayerEventInput playerEventInput;
    [SerializeField] private PlayerBattleInput playerBattleInput;
    
    public enum InputMode
    {
        Map,
        Cutscene,
        Battle,
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
                playerBattleInput.enabled = false;
                break;

            case InputMode.Cutscene:
                playerMapInput.enabled = false;
                playerEventInput.enabled = true;
                playerBattleInput.enabled = false;
                break;
            case InputMode.Battle:
                playerMapInput.enabled = false;
                playerEventInput.enabled = false;
                playerBattleInput.enabled = true;
                break;
        }
    }
}
