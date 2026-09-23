using System;
using UnityEngine;

[Serializable]
public class Scalpel : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        enemy.CurrentHp = Mathf.Clamp(enemy.CurrentHp - 40000, 0, enemy.CurrentMaxHp);
        enemy.CurrentAttack -= (int)(ally.CurrentAttack * 0.1);
    }
}
