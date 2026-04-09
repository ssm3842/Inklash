using UnityEngine;
using System.Collections;

public class Freeze : SpellBase
{
    [Header("Freeze Settings")]
    public GameObject rangeIndicatorPrefab;   // 범위 표시 이펙트
    public GameObject explosionEffectPrefab;   // 얼음 폭발 이펙트
    public float indicatorDuration = 1.5f;     // 범위 표시 지속 시간

    public override void CastSpell(float amount, float range, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, -0.7f, 0);
        StartCoroutine(FreezeSequence(amount, range));
    }

    private IEnumerator FreezeSequence(float amount, float range)
    {
        // 1. 범위 표시 이펙트 생성
        GameObject indicator = null;
        if (rangeIndicatorPrefab != null)
        {
            indicator = Instantiate(rangeIndicatorPrefab, transform.position, rangeIndicatorPrefab.transform.rotation);
            indicator.transform.localScale = rangeIndicatorPrefab.transform.localScale;
            // range에 맞게 스케일 조절 (필요시)
            // indicator.transform.localScale = Vector3.one * range;
        }

        // 2. 범위 표시 지속
        yield return new WaitForSeconds(indicatorDuration);

        // 3. 범위 표시 제거
        if (indicator != null) Destroy(indicator);

        // 4. 얼음 폭발 이펙트
        if (explosionEffectPrefab != null)
        {
            var explosion = Instantiate(explosionEffectPrefab, transform.position, explosionEffectPrefab.transform.rotation);
            explosion.transform.localScale = explosionEffectPrefab.transform.localScale;
            SetDepthSorting(explosion, transform.position.y);

            Destroy(explosion, 1f);
        }

        // 5. 범위 내 적 빙결
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(range, 100f), 0f);
        foreach (Collider2D enemy in enemies)
        {
            var damageable = enemy.GetComponent<DamageableObject>();
            if (damageable == null || damageable.isPlayers) continue;
            if (enemy.GetComponent<Units>() == null) continue;
            if (enemy.GetComponent<BuffController>() == null) continue;

            enemy.GetComponent<BuffController>().GetBuff(new BuffFreeze(amount));
            PerformHit(enemy, amount);
        }
    }
}