using UnityEngine;
using System.Collections;

public class ColdEffect : MonoBehaviour
{
    public string casterID;
    public int hitCount = 0;
    private float resetTimer = 5.0f;
    private float elapsed = 0f;

    public void AddStack()
    {
        hitCount++;
        elapsed = 0f; // 공격받을 때마다 타이머 초기화
        if (hitCount >= 3)
        {
            ApplyFreeze();
        }
    }

    private void ApplyFreeze()
    {
        // 피격자의 BuffController를 가져옵니다.
        BuffController bc = GetComponent<BuffController>();
        if (bc != null)
        {
            bc.GetBuff(new BuffFreeze(2f));
            
            Destroy(this); // 빙결 후 스택 초기화
        }
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= resetTimer)
        {
            Destroy(this); // 일정 시간 미타격 시 스택 소멸
        }
    }
}