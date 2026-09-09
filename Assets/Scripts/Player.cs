using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{   
    public event EventHandler<OnMoveChangedEventArgs> OnPlayerMove;

    public class OnMoveChangedEventArgs : EventArgs
    {
        public Vector2 moveVector;
        public bool canMove;
    }
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap obstaclesTilemap;
    
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector2 currentMoveVector;
    
    void Update()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        if (!isMoving)
        {
            currentMoveVector = playerInput.GetMoveDirections();
    
            bool isArrowPressed = currentMoveVector != Vector2.zero;
    
            if (isArrowPressed)
            {
                Vector3Int targetCell = grid.WorldToCell(transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0));
                Vector3Int targetCellX = grid.WorldToCell(transform.position + new Vector3(currentMoveVector.x, 0, 0));
                Vector3Int targetCellY = grid.WorldToCell(transform.position + new Vector3(0, currentMoveVector.y, 0));
                
                bool isWallInTarget = obstaclesTilemap.HasTile(targetCell);
                bool isWallInTargetX = obstaclesTilemap.HasTile(targetCellX);
                bool isWallInTargetY = obstaclesTilemap.HasTile(targetCellY);
    
                if (!isWallInTargetX && !isWallInTargetY && !isWallInTarget)
                {
                    OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = true });
                    targetPosition = grid.GetCellCenterWorld(targetCell);
                    isMoving = true;
                }
                else if (!isWallInTargetX)
                {
                    targetPosition = grid.GetCellCenterWorld(targetCellX);
                    OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = true });
                    isMoving = true;
                }
                else if (!isWallInTargetY)
                {
                    targetPosition = grid.GetCellCenterWorld(targetCellY);
                    OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = true });
                    isMoving = true;
                }
                else
                {
                    OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = false });
                }
            }
    
            // if (isArrowPressed)
            // {
            //     Vector3Int targetCell = grid.WorldToCell(transform.position + new Vector3(currentMoveVector.x, currentMoveVector.y, 0));
            //     
            //     bool isWallInTarget = obstaclesTilemap.HasTile(targetCell);
            //     
            //     targetPosition = grid.GetCellCenterWorld(targetCell);
            //
            //     if (!isWallInTarget)
            //     {
            //         OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = true });
            //         isMoving = true;
            //     }
            //     else
            //     {
            //         OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = currentMoveVector, canMove = false });
            //     }
            // }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,  moveSpeed * Time.deltaTime);
            
            if (transform.position == targetPosition)
            {
                OnPlayerMove?.Invoke(this, new OnMoveChangedEventArgs { moveVector = Vector2.zero, canMove = false });
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}