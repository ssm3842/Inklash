using UnityEngine;

public class BuffRage : Buffs
{
    public BuffRage(float newAmount = 0f)
    {
        buffName = "Rage";
        amount = newAmount;
        remainTime = 3f;
        order = 0;
    }
    public override void OnGetBuff() //버프를 받았을 때 효과 처리.
    {
        statController.ControlBonusStat(StatType.ATKSPD, amount);
        statController.ControlBonusStat(StatType.SPD, amount);
        statController.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
        statController.ControlBonusStat(StatType.ATKSPD, -amount);
        statController.ControlBonusStat(StatType.SPD, -amount);
        statController.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
