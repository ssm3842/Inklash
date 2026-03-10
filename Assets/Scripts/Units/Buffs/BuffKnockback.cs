using UnityEngine;

public class BuffKnockback : Buffs
{
    public BuffKnockback()
    {
        this.buffName = "Knockback";
        this.remainTime = -1;
    }

    public override void OnGetBuff(GameObject owner)
    {
       Units unit = owner.GetComponent<Units>();
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