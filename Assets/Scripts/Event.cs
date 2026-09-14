using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum StepType
{
    Phrase,
    GiveItem,
    Battle
}

[Serializable]
public class Step
{
    public StepType stepType;
    
    [ShowIf("stepType", StepType.Phrase)] [AllowNesting] public PhraseInfo phrase;
    [ShowIf("stepType", StepType.GiveItem)] [AllowNesting] public GiveItemInfo giveItem;
    [ShowIf("stepType", StepType.Battle)] [AllowNesting] public BattleInfo battle;
}

[Serializable]
public class Sequence
{
    [SerializeField] public List<Step> sequence;
    public bool isDestroys;
}

[Serializable]
public class Sequences
{
    [SerializeField] public List<Sequence> sequences;
}

public class Event : MonoBehaviour, IEvent
{
    [SerializeField] private AudioClip aaa;
    [SerializeField] private bool isOneTimeEvent;
    [SerializeField] private EventContext eventContext;
    [SerializeField] private Sequences eventData;
    
    private int _sequenceIndex, _stepIndex;
    private IEventStep _activeEventStep;
    private bool _isActivated = false;

    private void Update()
    {
        if (_activeEventStep != null) _activeEventStep.Update();
    }
    
    public void Execute(Player player)
    {
        if (eventData.sequences == null || _isActivated) return;
        eventContext.soundManager.SetMusic(aaa);
        _isActivated = true;
        _stepIndex = 0;
        eventContext.CoroutineRunner = this;
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Event);
        NextStep();
    }
    
    private void NextStep()
    {
        if (_stepIndex >= eventData.sequences[_sequenceIndex].sequence.Count)
        {
            End();
            return;
        }

        switch (eventData.sequences[_sequenceIndex].sequence[_stepIndex].stepType)
        {
            case StepType.Phrase:
                _activeEventStep = new TypePhrase();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                break;
            case StepType.GiveItem:
                _activeEventStep = new GiveItem();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                break;
            case StepType.Battle:
                _activeEventStep = new Battle();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                break;
        }
    }

    private void OnStepFinished()
    {
        _stepIndex++;
        NextStep();
    }
    
    private void End()
    {
        _isActivated = isOneTimeEvent;
        if (_sequenceIndex < eventData.sequences.Count-1)
        {
            _sequenceIndex++;
        }
        else if (eventData.sequences[_sequenceIndex].isDestroys)
        {
            Destroy(gameObject);
        }
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Map);
    }
}