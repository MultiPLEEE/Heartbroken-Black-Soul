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

public class EventPhrase : IEventStep
{
    private float _typingSpeed = 0.017f;
    private Coroutine _currentCoroutine;
    public bool IsRunning { get; private set; }

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        PhraseInfo info = step.phrase;

        switch (info.textPanelType)
        {
            case TextPanelType.Primary:
                eventContext.primaryPanelParent.SetActive(true);
                eventContext.primaryNamePanel.SetText(info.speakerName);
                eventContext.primaryPortraitPanel.sprite = info.speakerPortrait;
                break;
            case TextPanelType.Secondary:
                eventContext.secondaryPanelParent.SetActive(true);
                break;
            case TextPanelType.Third:
                eventContext.thirdPanelParent.SetActive(true);
                break;
        }
        
        _currentCoroutine = eventContext.CoroutineRunner.StartCoroutine(Coroutine(info, eventContext, onComplete));
    }
    
    private IEnumerator Coroutine(PhraseInfo info, EventContext eventContext, Action onComplete)
    { 
        IsRunning = true;

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

        IsRunning = false;
    }

    public void Update(Step step, EventContext eventContext, Action onComplete)
    {
        
    }
    
    public void Skip(Step step, EventContext eventContext, Action onComplete)
    {
        PhraseInfo info = step.phrase;
        
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
            return;
        }
        
        if (_currentCoroutine != null && eventContext.CoroutineRunner != null)
        {
            eventContext.CoroutineRunner.StopCoroutine(_currentCoroutine);
        }
        
        textPanel.SetText(info.speakerText);
        
        IsRunning = false;
    }
    
    public void End(Step step, EventContext eventContext, Action onComplete)
    {
        eventContext.primaryPanelParent.SetActive(false);
        eventContext.secondaryPanelParent.SetActive(false);
        eventContext.thirdPanelParent.SetActive(false);
        onComplete?.Invoke();
    }
}
