using UnityEngine;
using System.Collections;

public class PoisonEffect : MonoBehaviour
{
    public int hitCount = 0;        // 현재 쌓인 독 스택
    private float tickElapsed = 0f;  // 데미지 주기를 계산하는 타이머
    private float durationElapsed = 0f; // 마지막 타격 이후 경과 시간
    
    [SerializeField] private float effectDuration = 3.0f; // 독 효과 유지 시간 (3초간 추가 타격 없으면 삭제)

    public void AddStack()
    {
        hitCount++;        // 스택 증가
        durationElapsed = 0f; // 타격 시 유지 시간 초기화
    }

    void Update()
    {
        DamageableObject target = GetComponent<DamageableObject>();
        
        // 1. 유지 시간 체크 (일정 시간 동안 추가 타격이 없으면 컴포넌트 삭제)
        durationElapsed += Time.deltaTime;
        if (durationElapsed >= effectDuration || hitCount <= 0)
        {
            Destroy(this);
            return;
        }

        // 2. 데미지 주기 체크 (1초마다 스택만큼 데미지)
        tickElapsed += Time.deltaTime;
        if (tickElapsed >= 1.0f)
        {
            if (target != null) 
            {
                // 현재 쌓인 hitCount(스택)만큼 데미지를 입힘
                StartCoroutine(target.TakeDamage(hitCount, 0f));
            }
            
            tickElapsed = 0f; // 타이머 초기화
        }
    }
}