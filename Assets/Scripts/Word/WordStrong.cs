using UnityEngine;

public class WordString : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new WordCardStrong());
    }
}
