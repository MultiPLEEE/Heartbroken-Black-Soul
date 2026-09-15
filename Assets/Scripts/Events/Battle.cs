using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MenuType
{
    ActionSelect,
    ItemSelect
}

[Serializable]
public class BattleInfo
{
    public GameObject actionSelectParent;
    public ActionSlotUI actionSlotPrefab;
    public Transform actionGridContainer;
    
    public GameObject itemSelectParent;
    public Transform itemGridContainer;
    public InventorySlotUI itemSlotPrefab;
    public TMP_Text itemDescriptionText;

    [NonSerialized] public List<ActionType> Actions = new List<ActionType>{
        ActionType.Attack,
        ActionType.Technique,
        ActionType.Guard,
        ActionType.Items,
        ActionType.Run
    };
    public Inventory inventory;
    
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

public class Battle : IEventStep
{
    private MenuType _currentMenu;
    private int _currentItemIndex;
    private int _currentActionIndex;
    private List<ActionSlotUI> _spawnedActionSlots = new List<ActionSlotUI>();
    private List<InventorySlotUI> _spawnedItemSlots = new List<InventorySlotUI>();
    
    private BattleInfo _info;
    private EventContext _context;
    private Action _onComplete;
    
    private bool _isWaitingForPlayerTurn;

    private void PlayerEventInputOnPlayerConfirm(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                switch (_info.Actions[_currentActionIndex])
                {
                    case ActionType.Attack:
                        Debug.Log("Attack!");
                        EndPlayerTurn();
                        break;
                    case ActionType.Guard:
                        Debug.Log("Guard!");
                        EndPlayerTurn();
                        break;
                    case ActionType.Items:
                        ActivateItemMenu();
                        Debug.Log("ToItemSelect!");
                        break;
                }
                break;
            case MenuType.ItemSelect:
                _info.inventory.GetInventorySlots()[_currentItemIndex].item.effect.Execute(_info.AllyUnit, _info.EnemyUnit);
                
                bool isTakesTurn = _info.inventory.GetInventorySlots()[_currentItemIndex].item.isTakesTurn;
                
                _info.inventory.RemoveItem(_info.inventory.GetInventorySlots()[_currentItemIndex].item);
                if (isTakesTurn)
                {
                    EndPlayerTurn();
                }
                else
                {
                    ActivateActionMenu();
                }
                break;
        }
    }

    private void PlayerEventInputOnPlayerPrevious(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                ActivateActionMenu();
                break;
        }
    }
    
    private void PlayerEventInputOnPlayerUp(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                do
                {
                    if (_currentActionIndex > 0) _currentActionIndex--;
                    else _currentActionIndex = _info.Actions.Count - 1;
                } while (_spawnedActionSlots[_currentActionIndex].IsDisabled);
                UpdateActionSelectionVisuals();
                break;
            case MenuType.ItemSelect:
                if (_currentItemIndex > 1) _currentItemIndex -= 2;
                UpdateItemSelectionVisuals();
                break;
        }
    }
    
    private void PlayerEventInputOnPlayerDown(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                do
                {
                    if (_currentActionIndex < _info.Actions.Count-1) _currentActionIndex++;
                    else _currentActionIndex = 0;
                } while (_spawnedActionSlots[_currentActionIndex].IsDisabled);
                UpdateActionSelectionVisuals();
                break;
            case MenuType.ItemSelect:
                if (_currentItemIndex < _info.inventory.GetInventorySlots().Count-2) _currentItemIndex += 2;
                UpdateItemSelectionVisuals();
                break;
        }
    }
    
    private void PlayerEventInputOnPlayerLeft(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                if (_currentItemIndex > 0) _currentItemIndex--;
                UpdateItemSelectionVisuals();
                break;
        }
    }
    
    private void PlayerEventInputOnPlayerRight(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                // Nothing
                break;
            case MenuType.ItemSelect:
                if (_currentItemIndex < _info.inventory.GetInventorySlots().Count-1) _currentItemIndex++;
                UpdateItemSelectionVisuals();
                break;
        }
    }

    public void Execute(Step step, EventContext eventContext, Action onComplete)
    {
        _info = step.battle;
        _context =  eventContext;
        _onComplete = onComplete;
        
        _info.Initialize();
        
        eventContext.battleUIParent.SetActive(true);
    }

    public void Update()
    {
        if (_isWaitingForPlayerTurn) return;

        if (_info.AllyUnit.IsReadyToAct)
        {
            Debug.Log("Ally is acting!");
            StartPlayerTurn();
        }
        else if (_info.EnemyUnit.IsReadyToAct)
        {
            Debug.Log("Enemy is acting!");
            _info.EnemyUnit.ResetGauge();
        }
        else
        {
            float delta = Time.deltaTime;

            _info.AllyUnit.TickActionGauge(delta);
            _info.EnemyUnit.TickActionGauge(delta);
        }
    }
    
    private void RenderActionUI()
    {
        foreach (Transform child in _info.actionGridContainer)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
        _spawnedActionSlots.Clear();
        
        List<ActionType> slots = _info.Actions;
        
        var inventorySlots = _info.inventory.GetInventorySlots();
        bool isInventoryEmpty = inventorySlots == null || inventorySlots.Count == 0;
        
        for (int i = 0; i < slots.Count; i++)
        {
            ActionSlotUI slotUI = UnityEngine.Object.Instantiate(_info.actionSlotPrefab, _info.actionGridContainer);
            slotUI.Setup(slots[i]);
            if (slots[i] == ActionType.Items && isInventoryEmpty) slotUI.SetDisable(true);
            _spawnedActionSlots.Add(slotUI);
        }

        UpdateActionSelectionVisuals();
    }
    
    private void UpdateActionSelectionVisuals()
    {
        for (int i = 0; i < _spawnedActionSlots.Count; i++)
        {
            _spawnedActionSlots[i].SetSelected(i == _currentActionIndex);
        }
    }

    private void RenderInventoryUI()
    {
        foreach (Transform child in _info.itemGridContainer)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
        _spawnedItemSlots.Clear();
        
        List<InventorySlot> slots = _info.inventory.GetInventorySlots();
        
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlotUI slotUI = UnityEngine.Object.Instantiate(_info.itemSlotPrefab, _info.itemGridContainer);
            slotUI.Setup(slots[i]);
            _spawnedItemSlots.Add(slotUI);
        }

        UpdateItemSelectionVisuals();
    }
    
    private void UpdateItemSelectionVisuals()
    {
        List<InventorySlot> slots = _info.inventory.GetInventorySlots();
        
        for (int i = 0; i < _spawnedItemSlots.Count; i++)
        {
            _spawnedItemSlots[i].SetSelected(i == _currentItemIndex);
        }
        
        _info.itemDescriptionText.SetText(
            $"{slots[_currentItemIndex].item.objectEffectDescription}\n{slots[_currentItemIndex].item.objectDescription}");
    }

    private void StartPlayerTurn()
    {
        _isWaitingForPlayerTurn =  true;
        SubscribeInput();
        ActivateActionMenu();
    }
    
    private void EndPlayerTurn()
    {
        _isWaitingForPlayerTurn = false;
        UnsubscribeInput();
        CloseAllMenu();
        _info.AllyUnit.ResetGauge();
    }
    
    private void ActivateActionMenu()
    {
        CloseAllMenu();
        _currentMenu = MenuType.ActionSelect;
        _info.actionSelectParent.SetActive(true);
        RenderActionUI();
    }
    
    private void ActivateItemMenu()
    {
        CloseAllMenu();
        _currentMenu = MenuType.ItemSelect;
        _info.itemSelectParent.SetActive(true);
        RenderInventoryUI();
    }
    
    private void CloseAllMenu()
    {
        _currentItemIndex = 0;
        _currentActionIndex = 0;
        _info.actionSelectParent.SetActive(false);
        _info.itemSelectParent.SetActive(false);
    }
    
    private void SubscribeInput()
    {
        _context.playerEventInput.OnPlayerConfirm += PlayerEventInputOnPlayerConfirm;
        _context.playerEventInput.OnPlayerPrevious += PlayerEventInputOnPlayerPrevious;
        _context.playerEventInput.OnPlayerUp += PlayerEventInputOnPlayerUp;
        _context.playerEventInput.OnPlayerDown += PlayerEventInputOnPlayerDown;
        _context.playerEventInput.OnPlayerLeft += PlayerEventInputOnPlayerLeft;
        _context.playerEventInput.OnPlayerRight += PlayerEventInputOnPlayerRight;
    }
    
    private void UnsubscribeInput()
    {
        _context.playerEventInput.OnPlayerConfirm -= PlayerEventInputOnPlayerConfirm;
        _context.playerEventInput.OnPlayerPrevious -= PlayerEventInputOnPlayerPrevious;
        _context.playerEventInput.OnPlayerUp -= PlayerEventInputOnPlayerUp;
        _context.playerEventInput.OnPlayerDown -= PlayerEventInputOnPlayerDown;
        _context.playerEventInput.OnPlayerLeft -= PlayerEventInputOnPlayerLeft;
        _context.playerEventInput.OnPlayerRight -= PlayerEventInputOnPlayerRight;
    }

    private void End()
    {
        UnsubscribeInput();
        _onComplete?.Invoke();
    }
}