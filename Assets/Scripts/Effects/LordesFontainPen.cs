using System;

[Serializable]
public class LordesFontainPen : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentMaxHp += (int)(ally.CurrentMaxHp * 0.1);
        ally.CurrentHp += (int)(ally.CurrentHp * 0.1);
        
        ally.CurrentMaxMp += (int)(ally.CurrentMaxMp * 0.1);
        ally.CurrentMp += (int)(ally.CurrentMp * 0.1);
    }
}
