using UnityEngine;
using System.Collections;

public class Fireball : SpellBase
{
    [Header("Fireball Settings")]
    public GameObject fireballProjectilePrefab; // 낙하하는 화염구 프리팹
    public GameObject explosionEffectPrefab;     // 폭발 이펙트 프리팹

    [Header("Spawn Settings")]
    public int fireballCount = 5;
    public float spawnInterval = 0.15f;          // 생성 간격
    public float spawnAreaWidth = 3f;            // X축 랜덤 범위
    public float spawnAreaHeight = 1.5f;         // Y축 랜덤 범위 (2.5D 깊이감)
    public float impactBaseY = -0.5f; // 착탄 기준 Y 좌표

    [Header("Fall Settings")]
    public float fallDistance = 4f;              // 고정 낙하 거리
    public float fallSpeed = 8f;
    public Vector2 fallDirection = new Vector2(1f, -1f); // 대각선 방향

    public override void CastSpell(float damage, float range, float castXPosition)
    {
        StartCoroutine(SpawnFireballs(damage, range, castXPosition));
    }

    private IEnumerator SpawnFireballs(float damage, float range, float castXPosition)
    {
        Vector2 dir = fallDirection.normalized;

        for (int i = 0; i < fireballCount; i++)
        {
            // 범위 내 랜덤 착탄 지점
            float randX = castXPosition + Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
            float randY = impactBaseY + Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
            Vector3 impactPos = new Vector3(randX, randY, 0);

            // 착탄 지점에서 낙하 방향 역추적 → 시작 위치
            Vector3 startPos = impactPos - (Vector3)(dir * fallDistance);

            StartCoroutine(FallSingle(startPos, impactPos, dir, damage, range));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator FallSingle(Vector3 from, Vector3 to, Vector2 dir, float damage, float range)
    {
        // 화염구 프로젝타일 생성
        GameObject proj = (fireballProjectilePrefab != null)
            ? Instantiate(fireballProjectilePrefab, from, Quaternion.identity)
            : null;

        // 낙하 방향으로 회전 
        if (proj != null) 
        { 
            SetDepthSorting(proj, to.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f; 
            proj.transform.rotation = Quaternion.Euler(0, 0, angle); 
        }

        float duration = fallDistance / fallSpeed;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            if (proj != null)
                proj.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        // 화염구 제거 & 폭발 이펙트
        if (proj != null)
        {
            // 새 파티클 방출 중지
            var ps = proj.GetComponentInChildren<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = false;
            }
            // 잔여 파티클이 사라질 시간 후 제거
            Destroy(proj, 0.5f);
        }
        if (explosionEffectPrefab != null)
        {
            var explosion = Instantiate(explosionEffectPrefab, to, explosionEffectPrefab.transform.rotation);
            explosion.transform.localScale = explosionEffectPrefab.transform.localScale;
            SetDepthSorting(explosion, to.y);
        }

        // 착탄 지점 범위 데미지
        Collider2D[] enemies = Physics2D.OverlapBoxAll(new Vector2(to.x, 0f), new Vector2(range, 100f), 0f);
        foreach (Collider2D enemy in enemies)
        {
            var damageable = enemy.GetComponent<DamageableObject>();
            if (damageable == null || damageable.isPlayers) continue;
            StartCoroutine(damageable.TakeDamage(damage));
        }
    }
}