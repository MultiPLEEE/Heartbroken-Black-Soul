using System;

[Serializable]
public class FairyWing : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentSpeed += (int)(ally.CurrentSpeed * 0.1);
    }
}
