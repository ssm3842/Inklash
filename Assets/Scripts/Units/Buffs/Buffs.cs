using UnityEngine;

public class Buffs
{
    public string buffName; //버프 이름
    public float amount; //버프의 각종 값(분노의 속도값, 빙결의 지속 시간 등.)
    public float remainTime; //남은 시간을 계산할 때 사용
    public int order; //행동을 계산할 때 우선도 적용. 1순위: 기절, 빙결 / 2순위: 둔화 / 3순위: 미정 / 0: 영향 없음.

    public StatController statController;

    public bool CheckBuffValid()
    {
        if(remainTime == -1) return true;
        else if(remainTime <= 0) 
        {
            OnBuffEnd();
            return false;
        }
        else
        {
            remainTime -= Time.deltaTime;
            return true;
        }
    }
   public virtual void OnGetBuff(DamageableObject target)
    {
        // 1. 유닛인 경우
        if (target is Units unit)
        {
            ApplyUnit(unit);
        }
        // 2. 마법인 경우
        else if (target.GetComponent<SpellBase>() != null)
        {
            ApplySpell(target.GetComponent<SpellBase>());
        }
    }
    public virtual void OnBuffEnd() //버프 끝났을 때 처리.
    {
        return;
    }

    protected virtual void ApplyUnit(Units unit) { }
    protected virtual void ApplySpell(SpellBase spell) { }
}
