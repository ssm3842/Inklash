using UnityEngine;

public class WordQuick : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new WordCardQuick());
    }
}
