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
        if(remainTime <= 0) 
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

    public virtual void OnGetBuff() //버프를 받았을 때 효과 처리.
    {
        return;
    }
    public virtual void OnBuffEnd() //버프 끝났을 때 처리.
    {
        return;
    }
}
