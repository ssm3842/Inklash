using UnityEngine;

public class WordStrong : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new WordCardStrong());
    }
}
