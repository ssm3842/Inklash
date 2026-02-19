using UnityEngine;

public class WordCardHealthy : Buffs
{
    public WordCardHealthy()
    {
        buffName = "WordCardHealthy";
        remainTime = -1f;
        order = 1;
    }
    public override void OnGetBuff(DamageableObject owner) //버프를 받았을 때 효과 처리.
    {
        statController.ControlBonusStat(StatType.MAX_HP, 1f);
        statController.InitMaxHP();
        statController.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
    }
}
