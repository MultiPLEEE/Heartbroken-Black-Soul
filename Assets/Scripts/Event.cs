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
    [SerializeField] private PlayerEventInput playerEventInput;
    [SerializeField] private bool isOneTimeEvent;
    [SerializeField] private EventContext eventContext;
    [SerializeField] private Sequences eventData;
    
    private int _sequenceIndex, _stepIndex;
    private IEventStep _activeEventStep;
    private bool _isActivated = false;

    private void PlayerEventInput_OnPlayerConfirm(object sender, EventArgs e)
    {
        switch (eventData.sequences[_sequenceIndex].sequence[_stepIndex].stepType)
        {
            case StepType.Phrase:
                if (_activeEventStep.IsRunning)
                {
                    _activeEventStep.Skip(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                }
                else
                {
                    _activeEventStep.End(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                    NextStep();
                }
                break;
            case StepType.GiveItem:
                break;
            case StepType.Battle:
                break;
        }
    }

    private void Update()
    {
        if (_activeEventStep != null) _activeEventStep.Update(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);

    }
    
    public void Execute(Player player)
    {
        if (eventData.sequences == null || _isActivated) return;
        StartEvent();
    }
    
    private void StartEvent()
    {
        _isActivated = true;
        _stepIndex = 0;
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Cutscene);
        playerEventInput.OnPlayerConfirm += PlayerEventInput_OnPlayerConfirm;
        eventContext.CoroutineRunner = this;
        NextStep();
    }
    
    private void NextStep()
    {
        if (_stepIndex >= eventData.sequences[_sequenceIndex].sequence.Count)
        {
            EndCutscene();
            return;
        }

        switch (eventData.sequences[_sequenceIndex].sequence[_stepIndex].stepType)
        {
            case StepType.Phrase:
                _activeEventStep = new EventPhrase();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                break;
            case StepType.GiveItem:
                _activeEventStep = new EventGiveItem();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                _activeEventStep.End(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                NextStep();
                break;
            case StepType.Battle:
                _activeEventStep = new EventBattle();
                _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
                break;
        }
    }

    private void OnStepFinished()
    {
        _stepIndex++;
    }
    
    private void EndCutscene()
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
        playerEventInput.OnPlayerConfirm -= PlayerEventInput_OnPlayerConfirm;
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Map);
    }
}