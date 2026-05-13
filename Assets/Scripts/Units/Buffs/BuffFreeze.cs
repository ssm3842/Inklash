using UnityEngine;

public class BuffFreeze : Buffs
{
    public BuffFreeze(float newRemainTime = 0f)
    {
        buffName = "Freeze";
        remainTime = newRemainTime;
        order = 1;
    }
    public override void OnGetBuff(GameObject owner) //버프를 받았을 때 효과 처리.
                                                     //한상준 수정
                                                     //null과 관련된 버그가 터져서 gpt써서 새로 고쳤음.
    {
        if (owner == null)
        {
            Debug.LogError("owner가 null입니다.");
            return;
        }

        EffectController effect = owner.GetComponent<EffectController>();

        if (effect == null)
        {
            Debug.LogError(owner.name + " 오브젝트에 EffectController가 없습니다.");
            return;
        }

        effect.SetFreezeEffect(true);

        //statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(true);
        //위는 기존 코드
    }
    public override void OnBuffEnd() //버프 끝났을 때 처리.
                                     //한상준 수정
                                     //null과 관련된 버그가 터져서 gpt써서 새로 고쳤음
    {
        if (statController == null)
        {
            Debug.LogError("statController가 null입니다.");
            return;
        }

        EffectController effect = statController.GetComponent<EffectController>();

        if (effect == null)
        {
            Debug.LogError("EffectController가 없습니다.");
            return;
        }

        effect.SetFreezeEffect(false);

        //statController.gameObject.GetComponent<EffectController>().SetFreezeEffect(false);
        //위는 기존 코드
    }
}
