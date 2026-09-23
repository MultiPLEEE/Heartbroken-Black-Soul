using System;

[Serializable]
public class ScrapIronCandy : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentAttack += (int)(ally.CurrentAttack * 0.1);
    }
}
