using UnityEngine;

public class BuffRage : Buffs
{
    public BuffRage()
    {
        buffName = "Rage";
        remainTime = 3f;
        order = 0;
    }
    public override void OnGetBuff() //버프를 받았을 때 효과 처리.
    {
        statController.ControlBonusStat(StatType.ATKSPD, 1f);
        statController.ControlBonusStat(StatType.SPD, 1f);
        statController.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
        statController.ControlBonusStat(StatType.ATKSPD, -1f);
        statController.ControlBonusStat(StatType.SPD, -1f);
        statController.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
