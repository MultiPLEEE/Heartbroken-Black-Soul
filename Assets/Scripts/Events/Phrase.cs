using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public enum TextPanelType
{
    Primary,
    Secondary,
    Third
}

[Serializable]
public class PhraseInfo
{
    public TextPanelType textPanelType;
    [ShowIf("textPanelType", TextPanelType.Primary)] [AllowNesting] public string speakerName;
    [ShowIf("textPanelType", TextPanelType.Primary)] [AllowNesting] public Sprite speakerPortrait;
    [TextArea(2, 5)] public string speakerText;
}

public class Phrase : IEventStep
{
    public bool IsRunning { get; private set; }
    
    private PhraseInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    private float _typingSpeed = 0.017f;
    private bool _phraseComplete;
    private Coroutine _currentCoroutine;
    
    private void PlayerEventInput_OnPlayerConfirm(object sender, EventArgs e)
    {
        if (_phraseComplete)
        {
            End();
        }
        else
        {
            Skip();
        }
    }

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        IsRunning = true;
        
        _info = step.phrase;
        _context = eventContext;
        _onComplete = onComplete;
        
        SubscribeInput();

        switch (_info.textPanelType)
        {
            case TextPanelType.Primary:
                eventContext.primaryPanelParent.SetActive(true);
                eventContext.primaryNamePanel.SetText(_info.speakerName);
                eventContext.primaryPortraitPanel.sprite = _info.speakerPortrait;
                break;
            case TextPanelType.Secondary:
                eventContext.secondaryPanelParent.SetActive(true);
                break;
            case TextPanelType.Third:
                eventContext.thirdPanelParent.SetActive(true);
                break;
        }
        
        _currentCoroutine = eventContext.CoroutineRunner.StartCoroutine(Coroutine(_info, eventContext, onComplete));
    }
    
    private IEnumerator Coroutine(PhraseInfo info, EventContext eventContext, Action onComplete)
    { 
        _phraseComplete = false;

        TMP_Text textPanel = null;
        switch (info.textPanelType)
        {
            case TextPanelType.Primary:
                textPanel = eventContext.primaryTextPanel;
                break;
            case TextPanelType.Secondary:
                textPanel = eventContext.secondaryTextPanel;
                break;
            case TextPanelType.Third:
                textPanel = eventContext.thirdTextPanel;
                break;
        }

        if (textPanel == null)
        {
            IsRunning = false;
            yield break;
        }

        textPanel.SetText("");
    
        foreach (char letter in info.speakerText)
        {
            textPanel.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        _phraseComplete = true;
    }

    public void Update()
    {
        
    }
    
    private void Skip()
    {
        TMP_Text textPanel = null;
        switch (_info.textPanelType)
        {
            case TextPanelType.Primary:
                textPanel = _context.primaryTextPanel;
                break;
            case TextPanelType.Secondary:
                textPanel = _context.secondaryTextPanel;
                break;
            case TextPanelType.Third:
                textPanel = _context.thirdTextPanel;
                break;
        }

        if (textPanel == null)
        {
            IsRunning = false;
            return;
        }
        
        if (_currentCoroutine != null && _context.CoroutineRunner != null)
        {
            _context.CoroutineRunner.StopCoroutine(_currentCoroutine);
        }
        
        textPanel.SetText(_info.speakerText);
        
        _phraseComplete = true;
    }
    
    private void SubscribeInput()
    {
        _context.playerEventInput.OnPlayerConfirm += PlayerEventInput_OnPlayerConfirm;
    }
    
    private void UnsubscribeInput()
    {
        _context.playerEventInput.OnPlayerConfirm -= PlayerEventInput_OnPlayerConfirm;
    }
    
    private void End()
    {
        UnsubscribeInput();
        IsRunning = false;
        _context.primaryPanelParent.SetActive(false);
        _context.secondaryPanelParent.SetActive(false);
        _context.thirdPanelParent.SetActive(false);
        _onComplete?.Invoke();
    }
}
