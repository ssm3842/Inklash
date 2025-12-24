using UnityEngine;

public class BuffFreeze : Buffs
{
    public BuffFreeze()
    {
        buffName = "Freeze";
        remainTime = 2f;
        order = 1;
    }
    public override void OnGetBuff() //버프를 받았을 때 효과 처리.
    {
        statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(true);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
        statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(false);
    }
}
