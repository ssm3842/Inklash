using UnityEngine;

public class WordFast : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new WordCardFast());
    }
}
