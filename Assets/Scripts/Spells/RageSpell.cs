using UnityEngine;

public class Rage : SpellBase
{
    float mulScale = 0f;
    public override void CastSpell(float amount, float range, float castXPosition) //TODO: range가 반영되게끔 수정.
    {
        mulScale = amount;
        transform.position = new Vector3(castXPosition, -0.85f, 0);
    }
    void OnTriggerStay2D(Collider2D unit)
    {
        if(!unit.gameObject.GetComponent<Units>().isPlayers) return; //적은 효과 받지 않음.
        if(unit.gameObject.GetComponent<BuffController>() == null) return;
        unit.gameObject.GetComponent<BuffController>().GetBuff(new BuffRage(mulScale));
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(3f, 0.8f));
    }
}
