using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // private const string IS_WALKING = "IsWalking";
    // private const string MOVEX = "MoveX";
    // private const string MOVEY = "MoveY";
    //
    // [SerializeField] private Player player;
    //
    // private Animator animator;
    //
    // private void Awake()
    // {
    //     animator = GetComponent<Animator>();
    // }
    //
    // private void Start()
    // {
    //     player.OnPlayerMove += Player_OnPlayerMove;
    // }
    //
    // private void Player_OnPlayerMove(object sender, Player.OnMoveChangedEventArgs e)
    // {
    //     if (e.moveVector != Vector2.zero)
    //     {
    //         animator.SetFloat(MOVEX, e.moveVector.x);
    //         animator.SetFloat(MOVEY, e.moveVector.y);
    //         animator.SetBool(IS_WALKING, e.canMove);
    //     }
    //     else
    //     {
    //         animator.SetBool(IS_WALKING, false);
    //     }
    // }
}
