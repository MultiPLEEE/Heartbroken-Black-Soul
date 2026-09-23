using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum StepType
{
    Phrase,
    GiveItem,
    Battle,
    MoveCamera,
    PlaySound,
    SetMusic,
    SetCG,
    Wait,
    SpawnObject,
    DestroyObject,
    QuitGame
}

[Serializable]
public class Step
{
    public StepType stepType;
    
    [ShowIf("stepType", StepType.Phrase)] [AllowNesting] public PhraseInfo phrase;
    [ShowIf("stepType", StepType.GiveItem)] [AllowNesting] public GiveItemInfo giveItem;
    [ShowIf("stepType", StepType.Battle)] [AllowNesting] public BattleInfo battle;
    [ShowIf("stepType", StepType.MoveCamera)] [AllowNesting] public MoveCameraInfo moveCamera;
    [ShowIf("stepType", StepType.PlaySound)] [AllowNesting] public PlaySoundInfo playSound;
    [ShowIf("stepType", StepType.SetMusic)] [AllowNesting] public SetMusicInfo setMusic;
    [ShowIf("stepType", StepType.SetCG)] [AllowNesting] public SetCGInfo setCG;
    [ShowIf("stepType", StepType.Wait)] [AllowNesting] public WaitInfo wait;
    [ShowIf("stepType", StepType.SpawnObject)] [AllowNesting] public SpawnObjectInfo spawnObject;
    [ShowIf("stepType", StepType.DestroyObject)] [AllowNesting] public DestroyObjectInfo destroyObject;
    [ShowIf("stepType", StepType.QuitGame)] [AllowNesting] public quitGameInfo quitGame;
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
                _activeEventStep = new Phrase();
                break;
            case StepType.GiveItem:
                _activeEventStep = new ItemGet();
                break;
            case StepType.Battle:
                _activeEventStep = new Battle();
                break;
            case StepType.MoveCamera:
                _activeEventStep = new MoveCamera();
                break;
            case StepType.PlaySound:
                _activeEventStep = new PlaySound();
                break;
            case StepType.SetMusic:
                _activeEventStep = new SetMusic();
                break;
            case StepType.SetCG:
                _activeEventStep = new SetCG();
                break;
            case StepType.Wait:
                _activeEventStep = new Wait();
                break;
            case StepType.SpawnObject:
                _activeEventStep = new SpawnObject();
                break;
            case StepType.DestroyObject:
                _activeEventStep = new DestroyObject();
                break;
            case StepType.QuitGame:
                _activeEventStep = new QuitGame();
                break;
        }
        
        _activeEventStep.Execute(eventData.sequences[_sequenceIndex].sequence[_stepIndex], eventContext, OnStepFinished);
    }

    private void OnStepFinished()
    {
        _stepIndex++;
        NextStep();
    }
    
    private void End()
    {
        _activeEventStep = null; 
        _isActivated = false;
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