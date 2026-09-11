using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextPanelType
{
    Primary,
    Secondary,
    Third
}

[Serializable]
public struct CutsceneStep
{
    public TextPanelType textPanelType;
    public string speakerName;
    public Sprite speakerPortrait;
    [TextArea(2, 5)] public string speakerText;
    
    public bool autoPhraseProgress;
    public float autoPhraseProgressDelay;
}

[Serializable]
public struct CutsceneSequence
{
    [SerializeField] private string sequenceName;
    [SerializeField] private List<CutsceneStep> steps;

    public string SequenceName => sequenceName;
    public List<CutsceneStep> Steps => steps;
}

public class Cutscene : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private PlayerCutsceneInput playerCutsceneInput;
    
    [SerializeField] private List<CutsceneSequence> cutsceneData = new List<CutsceneSequence>();
    
    [SerializeField] private GameObject primaryPanelParent;
    [SerializeField] private TMP_Text primaryNamePanel;
    [SerializeField] private TMP_Text primaryTextPanel;
    [SerializeField] private Image primaryPortraitPanel;
    
    // [SerializeField] private GameObject secondaryPanelParent;
    // [SerializeField] private TMP_Text secondaryTextPanel;
    //
    // [SerializeField] private GameObject thirdPanel;
    // [SerializeField] private TMP_Text thirdTextPanel;
    
    private float typingSpeed = 3f;
    private int currentSequenceIndex, currentStepIndex;
    private bool isTyping;
    private Coroutine currentStepCoroutine;

    private void Start()
    {
        playerCutsceneInput.OnPlayerConfirm += PlayerCutsceneInput_OnPlayerConfirm;
    }

    private void PlayerCutsceneInput_OnPlayerConfirm(object sender, EventArgs e)
    {
        if(!isTyping) NextStep();
    }
    
    public void Interact(Player player)
    {
        if (cutsceneData == null) return;
        StartCutscene();
    }

    private void NextStep()
    {
        if (currentStepIndex >= cutsceneData[currentSequenceIndex].Steps.Count)
        {
            EndCutscene();
            return;
        }

        isTyping = true;
        // dialogueText.SetText(cutsceneData.text);

        // foreach (char letter in cutsceneData[cutsceneStepIndex].text)
        // {
        //     
        // }
        
        primaryNamePanel.SetText(cutsceneData[currentSequenceIndex].Steps[currentStepIndex].speakerName);
        primaryTextPanel.SetText(cutsceneData[currentSequenceIndex].Steps[currentStepIndex].speakerText);
        primaryPortraitPanel.sprite = cutsceneData[currentSequenceIndex].Steps[currentStepIndex].speakerPortrait;

        currentStepIndex++;
        isTyping = false;
    }
    
    private void StartCutscene()
    {
        currentStepIndex = 0;
        
        primaryPanelParent.SetActive(true);
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Cutscene);
        
        NextStep();
    }
    
    private void EndCutscene()
    {
        if (currentSequenceIndex < cutsceneData.Count-1) currentSequenceIndex++;
        primaryPanelParent.SetActive(false);
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Map);
    }
}