using UnityEngine;

public class WordCold : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new BuffWordCold()); 
    }
}