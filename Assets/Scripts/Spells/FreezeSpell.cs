using UnityEngine;

public class Freeze : SpellBase
{
    public override void CastSpell(float amount, float range, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, -0.5f, 0);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(range, 0.6f), 0f);

        foreach (Collider2D enemy in enemies)
        {
            if(enemy.gameObject.GetComponent<DamageableObject>().isPlayers) continue;
            if(enemy.gameObject.GetComponent<Units>() == null) continue;
            if(enemy.gameObject.GetComponent<BuffController>() == null) continue;
            
            enemy.gameObject.GetComponent<BuffController>().GetBuff(new BuffFreeze(amount));
            PerformHit(enemy,amount);

        }
    }
}

