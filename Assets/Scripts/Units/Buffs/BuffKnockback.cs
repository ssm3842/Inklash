using UnityEngine;

public class BuffKnockback : Buffs
{
    public BuffKnockback()
    {
        this.buffName = "Knockback";
        this.remainTime = -1;
    }

    public override void OnGetBuff(DamageableObject owner)
    {
       Units unit = owner as Units;
        if (unit != null)
        {
            unit.isKnockbackEnhanced = true;
            
            KnockBack kb = unit.GetComponent<KnockBack>();
            if (kb == null)
            {
                kb = unit.gameObject.AddComponent<KnockBack>();
            }
            
        }
    }
}