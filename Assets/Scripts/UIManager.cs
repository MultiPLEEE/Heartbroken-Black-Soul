using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject primaryPanelParent;
    public TMP_Text primaryNamePanel;
    public TMP_Text primaryTextPanel;
    public Image primaryPortraitPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
