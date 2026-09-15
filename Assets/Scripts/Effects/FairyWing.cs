using System;

[Serializable]
public class FairyWing : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentHp = ally.data.maxHp;
        ally.CurrentMp = ally.data.maxMp;
    }
}
