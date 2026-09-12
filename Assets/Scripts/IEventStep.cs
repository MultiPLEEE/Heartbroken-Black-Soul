using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class EventContext
{
    [field: NonSerialized] public MonoBehaviour CoroutineRunner;
    public Player player;
    public GameObject primaryPanelParent;
    public TMP_Text primaryNamePanel;
    public TMP_Text primaryTextPanel;
    public Image primaryPortraitPanel;
    public GameObject secondaryPanelParent;
    public TMP_Text secondaryTextPanel;
    public GameObject thirdPanelParent;
    public TMP_Text thirdTextPanel;
}

public interface IEventStep
{
    bool IsRunning { get; }
    void Execute(Step step, EventContext context, Action onComplete);
    void Skip(Step step, EventContext context, Action onComplete);
    void End(Step step, EventContext context, Action onComplete);
}