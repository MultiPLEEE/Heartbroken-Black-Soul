using System;

[Serializable]
public class LordesFontainPen : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentHp = ally.data.maxHp;
        ally.CurrentMp = ally.data.maxMp;
    }
}
