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
    
    private Animator _animator;
    private bool _isContinueMoving = false;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        Vector2 currentPlayerDirectionVector = player.GetCurrentPlayerDirectionVector();
        
        _animator.SetFloat(CURRENT_DIRECTION_X, currentPlayerDirectionVector.x);
        _animator.SetFloat(CURRENT_DIRECTION_Y, currentPlayerDirectionVector.y);
        if (player.GetIsMoving())
        {
            if (player.GetIsSprinting()) _animator.SetFloat(ANIMATION_SPEED_MULTIPLIER, animationSpeedMultiplier); else _animator.SetFloat(ANIMATION_SPEED_MULTIPLIER, 1);
            _animator.SetBool(IS_WALKING, true);
            _isContinueMoving = true;
        }
        else
        {
            if (_isContinueMoving)
            {
                _isContinueMoving = false;
            }
            else
            {
                _animator.SetBool(IS_WALKING, false);
            }
        }
    }
}
