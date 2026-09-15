using UnityEngine;
using System;

[Serializable]
public class GoddessBlood : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        Debug.Log("GoddessBlood");
        ally.CurrentHp = ally.data.maxHp;
        ally.CurrentMp = ally.data.maxMp;
    }
}
