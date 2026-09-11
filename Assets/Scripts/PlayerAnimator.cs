using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string CURRENT_DIRECTION_X = "CurrentDirectionX";
    private const string CURRENT_DIRECTION_Y = "CurrentDirectionY";
    private const string ANIMATION_SPEED_MULTIPLIER = "AnimationSpeedMultiplier";
    
    [SerializeField] private Player player;
    [SerializeField] private float animationSpeedMultiplier = 1.5f;
    
    private Animator animator;
    private bool isContinueMoving = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Vector2 currentPlayerDirectionVector = player.GetCurrentPlayerDirectionVector();
        
        animator.SetFloat(CURRENT_DIRECTION_X, currentPlayerDirectionVector.x);
        animator.SetFloat(CURRENT_DIRECTION_Y, currentPlayerDirectionVector.y);
        if (player.GetIsMoving())
        {
            if (player.GetIsSprinting()) animator.SetFloat(ANIMATION_SPEED_MULTIPLIER, animationSpeedMultiplier); else animator.SetFloat(ANIMATION_SPEED_MULTIPLIER, 1);
            animator.SetBool(IS_WALKING, true);
            isContinueMoving = true;
        }
        else
        {
            if (isContinueMoving)
            {
                isContinueMoving = false;
            }
            else
            {
                animator.SetBool(IS_WALKING, false);
            }
        }
    }
}
