using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{   
    // public event EventHandler<OnPlayerMoveEventArgs> OnPlayerMove;
    //
    // public class OnPlayerMoveEventArgs : EventArgs
    // {
    //     public Vector2 currentMoveVector;
    //     public bool isMoving;
    // }
    
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private LayerMask blockingLayer;

    [SerializeField] private Inventory inventory;
    public Inventory GetPlayerInventory() => inventory;
    
    private bool isMoving = false;
    private Vector2 currentPlayerDirectionVector = new Vector2(0, 1);
    private float currentMoveSpeed;

    void Awake()
    {
        currentMoveSpeed = walkSpeed;
    }
    
    void Start()
    {
        playerInput.OnPlayerInteract += PlayerInput_OnPlayerInteract;
    }
    
    private void PlayerInput_OnPlayerInteract(object sender, EventArgs e)
    {
        if (!isMoving)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(
                transform.position + new Vector3(currentPlayerDirectionVector.x, currentPlayerDirectionVector.y, 0),
                0.4f, blockingLayer.value);

            if (hitCollider != null)
            {
                if (hitCollider.TryGetComponent<InteractableObject>(out InteractableObject interactable))
                {
                    interactable.Interact(this);
                }
                else
                {
                    Debug.Log("Объект на пути не интерактивен");
                }
            }
        }
    }
    
    void Update()
    {
        currentMoveSpeed = playerInput.IsSprintPressed()? sprintSpeed : walkSpeed;
        PlayerMovement();
    }
    
     private void PlayerMovement()
     {
         Vector2 currentMoveVector = playerInput.GetPlayerMoveVector();
         
         if (!isMoving)
         {
             bool isArrowPressed = currentMoveVector != Vector2.zero;
             
             if (isArrowPressed)
             {
                 if (!(currentMoveVector.x != 0 && currentMoveVector.y != 0 || currentMoveVector.x == 0 && currentMoveVector.y == 0))
                 {
                     currentPlayerDirectionVector = currentMoveVector;
                 }
                 
                 // visualTransform.localPosition = -currentMoveVector;
                 // transform.position = transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0);
                 // isMoving = true;
                 
                 bool isWallInTarget = Physics2D.OverlapCircle(transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0), 0.4f, blockingLayer.value) != null;
                 bool isWallInTargetX = Physics2D.OverlapCircle(transform.position + new Vector3(currentMoveVector.x, 0, 0), 0.4f, blockingLayer.value) != null;
                 bool isWallInTargetY = Physics2D.OverlapCircle(transform.position + new Vector3(0, currentMoveVector.y, 0), 0.4f, blockingLayer.value) != null;
                 
                 if ((currentMoveVector.y != 0 && currentMoveVector.x == 0 && isWallInTargetY) || (currentMoveVector.x != 0 && currentMoveVector.y == 0 && isWallInTargetX)) return;
                 
                 if (!isWallInTargetX && !isWallInTargetY)
                 {
                     transform.position = transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0);
                     visualTransform.localPosition = -currentMoveVector;
                     isMoving = true;
                 }
                 else if (!isWallInTargetX)
                 {
                     transform.position = transform.position + new Vector3(currentMoveVector.x, 0, 0);
                     visualTransform.localPosition = new Vector3(-currentMoveVector.x, 0, 0);
                     isMoving = true;
                 }
                 else if (!isWallInTargetY)
                 {
                     transform.position = transform.position + new Vector3(0, currentMoveVector.y, 0);
                     visualTransform.localPosition = new Vector3(0, -currentMoveVector.y, 0);
                     isMoving = true;
                 }
                 else
                 {
                 
                 }
             }
         }
         else
         {
             visualTransform.localPosition = Vector3.MoveTowards(visualTransform.localPosition, Vector3.zero,  currentMoveSpeed * Time.deltaTime);
             
             if (visualTransform.localPosition == Vector3.zero)
             {
                 visualTransform.localPosition = Vector3.zero;
                 isMoving = false;
             }
         }
     }
     
     // private void OnDrawGizmos()
     // {
     //     // Устанавливаем цвет круга (например, зеленый)
     //     Gizmos.color = Color.green;
     //
     //     // Считаем позицию целевой клетки
     //     Vector3 targetCheckPos = transform.position + new Vector3(currentPlayerDirectionVector.x, currentPlayerDirectionVector.y, 0);
     //
     //     // Рисуем проволочную сферу радиусом 0.2f (укажи тот же радиус, что и в OverlapCircle)
     //     Gizmos.DrawWireSphere(targetCheckPos, 0.4f);
     // }
}