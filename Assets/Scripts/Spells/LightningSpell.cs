using UnityEngine;

public class Lightning : SpellBase
{
    static readonly Color DamageTextColor = new Color(0.25f, 0.85f, 1f);

    public float spawnYMin = -2f;
    public float spawnYMax = -0.4f;

    public override void CastSpell(float damage, float range, float castXPosition)
    {
        float randY = Random.Range(spawnYMin, spawnYMax);
        transform.position = new Vector3(castXPosition, randY, 0);
        ApplyDepthScale(gameObject, randY);
        SetDepthSorting(gameObject, randY);

        Collider2D[] enemies = Physics2D.OverlapBoxAll(new Vector2(castXPosition, 0f), new Vector2(range, 100f), 0f);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy == null) continue;
            
            var damageable = enemy.GetComponent<DamageableObject>();
            if (damageable == null || damageable.isPlayers) continue;
            enemy.GetComponent<BuffController>()?.GetBuff(new BuffShock());
            StartCoroutine(damageable.TakeDamage(damage, 0f, false, DamageTextColor));
            PerformHit(enemy);
        }

        Destroy(gameObject, 2f);
    }
}
