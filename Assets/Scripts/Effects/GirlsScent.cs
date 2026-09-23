using System;
using UnityEngine;

[Serializable]
public class GirlsScent : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        enemy.CurrentHp = Mathf.Clamp(enemy.CurrentHp - 200000, 0, enemy.CurrentMaxHp);
    }
}
