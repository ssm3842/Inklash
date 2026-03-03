using UnityEngine;

public class Fireball : SpellBase
{
    public override void CastSpell(float damage, float range, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, -0.5f, 0);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, new Vector2(range, 0.6f), 0f);

        foreach (Collider2D enemy in enemies)
        {
            if(enemy.gameObject.GetComponent<DamageableObject>().isPlayers) continue; //아군 제외
            if(enemy.gameObject.GetComponent<DamageableObject>() == null) continue; //데미지 계산이 불가능한 오브젝트 제외

            StartCoroutine(enemy.gameObject.GetComponent<DamageableObject>().TakeDamage(damage));
        }
    }
}

