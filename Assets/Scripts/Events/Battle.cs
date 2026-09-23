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
    
    public AllySo ally;
    public AllySo enemy;
    
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
    private float _timer;
    private bool _reload;

    private int _enemyTurnCounter;
    
    private bool _isWaitingForPlayerTurn;

    private void PlayerEventInputOnPlayerConfirm(object sender, EventArgs e)
    {
        switch (_currentMenu)
        {
            case MenuType.ActionSelect:
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiConfirm, 1f);
                switch (_info.Actions[_currentActionIndex])
                {
                    case ActionType.Attack:
                        SoundManager.Instance.PlaySound(SoundManager.Instance.Database.attack, 0.5f);
                        _timer += 0.5f;
                        _info.EnemyUnit.TakeDamage(_info.AllyUnit.CurrentAttack);
                        EndPlayerTurn();
                        break;
                    case ActionType.Guard:
                        _info.AllyUnit.IsGuard = true;
                        _timer += 0.5f;
                        EndPlayerTurn();
                        break;
                    case ActionType.Items:
                        ActivateItemMenu();
                        break;
                }
                break;
            case MenuType.ItemSelect:
                _info.inventory.GetInventorySlots()[_currentItemIndex].item.effect.Execute(_info.AllyUnit, _info.EnemyUnit);
                SoundManager.Instance.PlaySound(_info.inventory.GetInventorySlots()[_currentItemIndex].item.soundEffect);
                    
                bool isTakesTurn = _info.inventory.GetInventorySlots()[_currentItemIndex].item.isTakesTurn;
                _info.inventory.RemoveItem(_info.inventory.GetInventorySlots()[_currentItemIndex].item);
                
                if (isTakesTurn)
                {
                    _timer += 0.5f;
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiPrevious, 1f);
                ActivateActionMenu();
                break;
        }
    }
    
    private void PlayerEventInputOnPlayerUp(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiMoveCursor, 1f);
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
        SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiMoveCursor, 1f);
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiMoveCursor, 1f);
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.uiMoveCursor, 1f);
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

        _enemyTurnCounter = 1;
        
        _context.battleUIParent.SetActive(true);
    }

    public void Update()
    {
        RenderStats();

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        
        if (_reload)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.Database.enemyGunReload, 0.9f);
            _reload = false;
            _timer += 0.5f;
            return;
        }
        if (_isWaitingForPlayerTurn) return;

        if (_info.EnemyUnit.IsDead) End();
        
        if (_info.AllyUnit.IsDead)
        {
            Application.Quit();
        
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (_info.AllyUnit.IsReadyToAct)
        {
            StartPlayerTurn();
        }
        else if (_info.EnemyUnit.IsReadyToAct)
        {
            if (_enemyTurnCounter % 4 == 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.enemyGunAttack, 1f);
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.enemyAttack, 0.5f);
                _info.AllyUnit.TakeDamage((int)(_info.EnemyUnit.CurrentAttack * 2f));
                _timer += 0.5f;
                _enemyTurnCounter++;
                _reload = true;
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.attack, 0.5f);
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.enemyAttack, 0.5f);
                _info.AllyUnit.TakeDamage(_info.EnemyUnit.CurrentAttack);
                _timer += 0.05f;
                _enemyTurnCounter++;
            }
            _info.EnemyUnit.ResetGauge();
        }
        else
        {
            float delta = Time.deltaTime;
            _info.AllyUnit.TickActionGauge(delta);
            _info.EnemyUnit.TickActionGauge(delta);
        }
    }

    private void RenderStats()
    {
        _context.hpValues.SetText($"{_info.AllyUnit.CurrentHp}/{_info.AllyUnit.CurrentMaxHp}");
        _context.mpValues.SetText($"{_info.AllyUnit.CurrentMp}");
        _context.apPercent.SetText($"{(int)_info.AllyUnit.ActionGauge}%");
        _context.hpBar.value = (float)_info.AllyUnit.CurrentHp / _info.AllyUnit.CurrentMaxHp;
        _context.mpBar.value = (float)_info.AllyUnit.CurrentMp / _info.AllyUnit.CurrentMaxMp;
        _context.apBar.value = _info.AllyUnit.ActionGauge / 100;
        
        _context.enemyHpBar.value = (float)_info.EnemyUnit.CurrentHp / _info.EnemyUnit.CurrentMaxHp;
        _context.enemyApBar.value = _info.EnemyUnit.ActionGauge / 100;
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
        _info.AllyUnit.IsGuard = false;
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
        _context.battleUIParent.SetActive(false);
        _onComplete?.Invoke();
    }
}