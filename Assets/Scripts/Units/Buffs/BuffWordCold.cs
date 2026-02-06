public class BuffWordCold : Buffs
{
    public BuffWordCold()
    {
        buffName = "BuffWordCold";
        remainTime = -1f;
    }

    public override void OnGetBuff()
    {
        if (statController != null && !statController.gameObject.GetComponent<ColdAttackEffect>())
        {
            statController.gameObject.AddComponent<ColdAttackEffect>();
        }
    }

    public override void OnBuffEnd()
    {
    }
}