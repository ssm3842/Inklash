using UnityEngine;

public class WordCardQuick : Buffs
{
    public WordCardQuick()
    {
        buffName = "WordCardQuick";
        remainTime = -1f;
        order = 1;
    }
    public override void OnGetBuff(DamageableObject owner) //버프를 받았을 때 효과 처리.
    {
        statController.ControlBonusStat(StatType.ATKSPD, 1f);
        statController.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
    }
}
