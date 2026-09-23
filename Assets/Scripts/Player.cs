using System;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeedMultiplier;
    [SerializeField] private PlayerMapInput playerMapInput;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private LayerMask eventLayer;
    [SerializeField] private Inventory inventory;
    
    private bool _isMoving = false, _isSprinting = false;
    private float _currentMoveSpeed;
    private Vector2 _currentPlayerDirectionVector = new Vector2(0, 1);
    private float _timer;
    
    public Inventory GetPlayerInventory() => inventory;
    public bool GetIsMoving() => _isMoving;
    public bool GetIsSprinting() => _isSprinting;
    public Vector2 GetCurrentPlayerDirectionVector() => _currentPlayerDirectionVector;


    void Awake()
    {
        _currentMoveSpeed = walkSpeed;
    }
    
    void Start()
    {
        playerMapInput.OnPlayerInteract += PlayerMapInput_OnPlayerMapInteract;
    }
    
    private void PlayerMapInput_OnPlayerMapInteract(object sender, EventArgs e)
    {
        ExecuteEvent(transform.position + new Vector3(_currentPlayerDirectionVector.x, _currentPlayerDirectionVector.y, 0));
    }
    
    void Update()
    {
        ExecuteEvent(transform.position);
        _isSprinting = playerMapInput.IsSprintPressed()? true : false;
        _currentMoveSpeed = _isSprinting ? walkSpeed * sprintSpeedMultiplier : walkSpeed;

        if (_isMoving)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.Database.playerStep, 0.5f);
            }
        } 
        
        PlayerMovement();
    }
    
     private void PlayerMovement()
     {
         Vector2 currentMoveVector = playerMapInput.GetPlayerMoveVector();
         
         if (!_isMoving)
         {
             bool isArrowPressed = currentMoveVector != Vector2.zero;
             
             if (isArrowPressed)
             {
                 if (!(currentMoveVector.x != 0 && currentMoveVector.y != 0 || currentMoveVector.x == 0 && currentMoveVector.y == 0)) _currentPlayerDirectionVector = currentMoveVector;
                 if (currentMoveVector.x == -_currentPlayerDirectionVector.x) _currentPlayerDirectionVector = new Vector2(currentMoveVector.x, _currentPlayerDirectionVector.y);
                 if (currentMoveVector.y == -_currentPlayerDirectionVector.y) _currentPlayerDirectionVector = new Vector2(_currentPlayerDirectionVector.x, currentMoveVector.y);
                 
                 // visualTransform.localPosition = -currentMoveVector;
                 // transform.position = transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0);
                 // isMoving = true;
                 
                 bool isWallInTarget = Physics2D.OverlapCircle(transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0), 0.4f, blockingLayer.value) != null;
                 bool isWallInTargetX = Physics2D.OverlapCircle(transform.position + new Vector3(currentMoveVector.x, 0, 0), 0.4f, blockingLayer.value) != null;
                 bool isWallInTargetY = Physics2D.OverlapCircle(transform.position + new Vector3(0, currentMoveVector.y, 0), 0.4f, blockingLayer.value) != null;
                 
                 if ((currentMoveVector.y != 0 && currentMoveVector.x == 0 && isWallInTargetY) || (currentMoveVector.x != 0 && currentMoveVector.y == 0 && isWallInTargetX)) return;
                 
                 if (!isWallInTargetX && !isWallInTargetY && !isWallInTarget)
                 {
                     transform.position = transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0);
                     visualTransform.localPosition = -currentMoveVector;
                     _isMoving = true;
                 }
                 else if (!isWallInTargetX)
                 {
                     transform.position = transform.position + new Vector3(currentMoveVector.x, 0, 0);
                     visualTransform.localPosition = new Vector3(-currentMoveVector.x, 0, 0);
                     _isMoving = true;
                 }
                 else if (!isWallInTargetY)
                 {
                     transform.position = transform.position + new Vector3(0, currentMoveVector.y, 0);
                     visualTransform.localPosition = new Vector3(0, -currentMoveVector.y, 0);
                     _isMoving = true;
                 }
                 else
                 {
                 
                 }
             }
         }
         else
         {
             visualTransform.localPosition = Vector3.MoveTowards(visualTransform.localPosition, Vector3.zero,  _currentMoveSpeed * Time.deltaTime);

             if (_timer <= 0) _timer += _isSprinting? 0.2f : 0.3f;
                 
             if (visualTransform.localPosition == Vector3.zero)
             {
                 visualTransform.localPosition = Vector3.zero;
                 _isMoving = false;
             }
         }
     }
     
     private void ExecuteEvent(Vector2 targetTransform)
     {
         if (!_isMoving)
         {
             Collider2D executor = Physics2D.OverlapCircle(targetTransform, 0.4f);

             if (executor != null)
             {
                 if (executor.TryGetComponent<IEvent>(out IEvent executableEvent)) executableEvent.Execute(this);
             }
         }
     }
}