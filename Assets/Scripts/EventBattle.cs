using System;
using UnityEngine;


public enum MenuType
{
    ActionSelect,
    ItemSelect
}

public enum ActionType
{
    Attack, 
    Guard, 
    Items
}

[Serializable]
public class BattleInfo
{
    // public Image attack;
    // public Image guard;
    // public Image items;
    
    public GameObject actionSelectParent;
    public GameObject itemSelectParent;
    
    public Inventory inventory;
    
    public PlayerBattleInput playerBattleInput;
    public AllySO ally;
    public AllySO enemy;
    
    [NonSerialized] public Ally AllyUnit;
    [NonSerialized] public Ally EnemyUnit;
    
    public void Initialize()
    {
        AllyUnit = new Ally(ally);
        EnemyUnit = new Ally(enemy);
    }
}

public class EventBattle : IEventStep
{
    private MenuType _currentMenu;
    private ActionType _currentAction;
    private int _currentSlotIndex;
    
    private BattleInfo _currentBattle;
    private bool _isWaitingForPlayerInput;
    
    public bool IsRunning { get; private set; }

    private void PlayerBattleInput_OnPlayerConfirm(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                switch (_currentAction)
                {
                    case ActionType.Attack:
                        Debug.Log("Attack!");
                        _isWaitingForPlayerInput = false;
                        break;
                    case ActionType.Guard:
                        Debug.Log("Guard!");
                        _isWaitingForPlayerInput = false;
                        break;
                    case ActionType.Items:
                        ToItemSelect();
                        break;
                }
                break;
            case MenuType.ItemSelect:
                Debug.Log("Use Item!");
                break;
        }
    }

    private void PlayerBattleInput_OnPlayerPrevious(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                ToActionSelect();
                break;
        }
    }
    
    private void PlayerBattleInput_OnPlayerUp(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                break;
            case MenuType.ItemSelect:
                break;
        }
    }
    
    private void PlayerBattleInput_OnPlayerDown(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                break;
            case MenuType.ItemSelect:
                break;
        }
    }
    
    private void PlayerBattleInput_OnPlayerLeft(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                break;
        }
    }
    
    private void PlayerBattleInput_OnPlayerRight(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                break;
        }
    }

    public void Execute(Step step, EventContext context, Action onComplete)
    {
        IsRunning = true;
        _isWaitingForPlayerInput = false;
        _currentBattle = step.battle;
        _currentBattle.Initialize();
        
        context.battleUIParent.SetActive(true);
        PlayerInputManager.Instance.SetInputMode(PlayerInputManager.InputMode.Battle);
        SubscribeInput();
        ToActionSelect();
    }
    
    public void Update(Step step, EventContext eventContext, Action onComplete)
    {
        if (!IsRunning || _isWaitingForPlayerInput) return;

        float delta = Time.deltaTime;
        
        _currentBattle.AllyUnit.TickActionGauge(delta);
        _currentBattle.EnemyUnit.TickActionGauge(delta);

        if (_currentBattle.AllyUnit.IsReadyToAct)
        {
            _isWaitingForPlayerInput = true;
            
            switch (_currentMenu)
            {
                case MenuType.ActionSelect:
                    ToItemSelect();
                    Debug.Log("ItemSelect!");
                    break;
                case MenuType.ItemSelect:
                    Debug.Log("ItemSelect!");
                    break;
            }
            
            _currentBattle.AllyUnit.ResetGauge();
        }
        else if (_currentBattle.EnemyUnit.IsReadyToAct)
        {
            Debug.Log("Enemy is acting!");
            _currentBattle.EnemyUnit.ResetGauge();
        }
    }

    public void Skip(Step step, EventContext context, Action onComplete)
    {
        
    }

    public void End(Step step, EventContext context, Action onComplete)
    {
        UnsubscribeInput();
        onComplete?.Invoke();
    }
    
    private void ToActionSelect()
    {
        _currentMenu = MenuType.ActionSelect;
        _currentBattle.actionSelectParent.SetActive(true);
        _currentBattle.itemSelectParent.SetActive(false);
    }
    
    private void ToItemSelect()
    {
        _currentSlotIndex = 0;
        _currentMenu = MenuType.ItemSelect;
        _currentBattle.actionSelectParent.SetActive(false);
        _currentBattle.itemSelectParent.SetActive(true);
    }
    
    private void SubscribeInput()
    {
        _currentBattle.playerBattleInput.OnPlayerConfirm += PlayerBattleInput_OnPlayerConfirm;
        _currentBattle.playerBattleInput.OnPlayerPrevious += PlayerBattleInput_OnPlayerPrevious;
        _currentBattle.playerBattleInput.OnPlayerUp += PlayerBattleInput_OnPlayerUp;
        _currentBattle.playerBattleInput.OnPlayerDown += PlayerBattleInput_OnPlayerDown;
        _currentBattle.playerBattleInput.OnPlayerLeft += PlayerBattleInput_OnPlayerLeft;
        _currentBattle.playerBattleInput.OnPlayerRight += PlayerBattleInput_OnPlayerRight;
    }
    
    private void UnsubscribeInput()
    {
        _currentBattle.playerBattleInput.OnPlayerConfirm -= PlayerBattleInput_OnPlayerConfirm;
        _currentBattle.playerBattleInput.OnPlayerPrevious -= PlayerBattleInput_OnPlayerPrevious;
        _currentBattle.playerBattleInput.OnPlayerUp -= PlayerBattleInput_OnPlayerUp;
        _currentBattle.playerBattleInput.OnPlayerDown -= PlayerBattleInput_OnPlayerDown;
        _currentBattle.playerBattleInput.OnPlayerLeft -= PlayerBattleInput_OnPlayerLeft;
        _currentBattle.playerBattleInput.OnPlayerRight -= PlayerBattleInput_OnPlayerRight;
    }
}