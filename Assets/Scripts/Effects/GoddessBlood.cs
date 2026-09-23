using UnityEngine;
using System;

[Serializable]
public class GoddessBlood : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentHp = ally.CurrentMaxHp;
        ally.CurrentMp = ally.CurrentMaxMp;
    }
}
