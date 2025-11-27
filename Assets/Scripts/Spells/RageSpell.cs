using UnityEngine;

public class RageSpell : SpellBase
{
    public override void CastSpell(float damage, float castXPosition)
    {
        transform.position = new Vector3(castXPosition, -0.5f, 0);
    }
    void OnTriggerStay2D(Collider2D unit)
    {
        if(!unit.gameObject.GetComponent<Units>().isPlayers) return; //적은 효과 받지 않음.
        unit?.gameObject.GetComponent<Units>().buffController.GetBuff(ScriptableObject.CreateInstance<BuffRage>());
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(3f, 0.8f));
    }
}
