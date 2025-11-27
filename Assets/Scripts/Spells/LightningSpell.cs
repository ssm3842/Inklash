using UnityEngine;

public class Lightning : SpellBase
{
    public override void CastSpell(float damage, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, -0.5f, 0);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(2f, 0.6f), 0f);

        foreach (Collider2D enemy in enemies)
        {
            if(enemy.gameObject.GetComponent<Units>().isPlayers) continue;
            StartCoroutine(enemy?.gameObject.GetComponent<Units>().TakeDamage(100f));
        }
    }
}

