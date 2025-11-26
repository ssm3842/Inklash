using UnityEngine;

public class RageSpell : SpellBase
{
    public override void CastSpell(float damage, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, 0, 0);
        Collider2D[] units = Physics2D.OverlapBoxAll(transform.position, new Vector2(3f, 0.8f), 0f);

        foreach (Collider2D unit in units)
        {
            if(!unit.gameObject.GetComponent<Units>().isPlayers) continue; //적은 효과 받지 않음.
            unit?.gameObject.GetComponent<Units>().buffController.GetBuff(ScriptableObject.CreateInstance<BuffRage>());
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(3f, 0.8f));
    }
}
