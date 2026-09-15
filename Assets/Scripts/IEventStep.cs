using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EventContext
{
    [field: NonSerialized] public MonoBehaviour CoroutineRunner;
    public PlayerEventInput playerEventInput;
    public SoundManager soundManager;
    public SpriteRenderer CG;
    public GameObject cameraTarget;
    public Player player;
    public GameObject primaryPanelParent;
    public TMP_Text primaryNamePanel;
    public TMP_Text primaryTextPanel;
    public Image primaryPortraitPanel;
    public GameObject secondaryPanelParent;
    public TMP_Text secondaryTextPanel;
    public GameObject thirdPanelParent;
    public TMP_Text thirdTextPanel;
    public GameObject battleUIParent;
    public GameObject eventObjectParent;
}

public interface IEventStep
{
    void Execute(Step step, EventContext eventContext, Action onComplete);
    void Update();
}