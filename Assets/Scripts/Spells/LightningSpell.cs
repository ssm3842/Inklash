using UnityEngine;

public class Lightning : SpellBase
{
    public float spawnAreaHeight = 1f;
    public float impactBaseY = -0.8f;

    public override void CastSpell(float damage, float range, float castXPosition)
    {
        float randY = impactBaseY + Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f);
        transform.position = new Vector3(castXPosition, randY, 0);
        SetDepthSorting(gameObject, randY);

        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(range, 100f), 0f);
        foreach (Collider2D enemy in enemies)
        {
            var damageable = enemy.GetComponent<DamageableObject>();
            if (damageable == null || damageable.isPlayers) continue;
            enemy.GetComponent<BuffController>()?.GetBuff(new BuffShock());
            StartCoroutine(damageable.TakeDamage(damage));
            PerformHit(enemy);
        }
    }
}