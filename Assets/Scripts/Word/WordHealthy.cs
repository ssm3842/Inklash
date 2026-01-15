using UnityEngine;

public class WordHealthy : WordBase
{
    public override void ApplyBuff(BuffController targetUnit)
    {
        targetUnit.GetBuff(new WordCardHealthy());
    }
}
