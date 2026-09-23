using System;

[Serializable]
public class SomethingWhiteAndStinky : IEffect
{
    public void Execute(Ally ally, Ally enemy)
    {
        ally.CurrentMagic += (int)(ally.CurrentMagic * 0.1);
    }
}
