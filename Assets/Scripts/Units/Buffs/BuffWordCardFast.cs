using UnityEngine;

public class WordCardFast : Buffs
{
    public WordCardFast()
    {
        buffName = "WordCardFast";
        remainTime = -1f;
        order = 0;
    }
    public override void OnGetBuff() //버프를 받았을 때 효과 처리.
    {
        statController.ControlBonusStat(StatType.SPD, 1f);
        statController.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 1f);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
    }
}
