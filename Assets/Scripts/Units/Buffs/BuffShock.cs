using UnityEngine;

public class BuffShock : Buffs
{
    public BuffShock()
    {
        buffName = "Shock";
        remainTime = 0.5f;
        order = 1;
    }
    public override void OnGetBuff(GameObject owner) //버프를 받았을 때 효과 처리.
    {
        //감전 이펙트 넣기
        // statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(true);
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
    {
        // statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(false);
    }
}
