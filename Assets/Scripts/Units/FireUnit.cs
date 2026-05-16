using UnityEngine;

public class FireUnit : Units
{
    protected override void ApplyBurnEffect(DamageableObject hitTarget)
    {
        if (hitTarget == null) return;
        
        string myID = gameObject.GetInstanceID().ToString();
        BurnEffect effect = hitTarget.GetComponent<BurnEffect>();

        float baseDmg = hitTarget.GetComponent<StatController>().GetStat(StatType.MAX_HP) * 0.05f;
        float burnDmg =  HasBuff("Burn") ? baseDmg*2f : baseDmg;

        if (target.gameObject.name.Contains("Base"))
        {
            burnDmg *= 0.1f;
        }

        if (effect != null && effect.casterID == myID)
        {
            effect.damageAmount = burnDmg;
            effect.ResetTimer();
        }
        else
        {
            BurnEffect newFX = hitTarget.gameObject.AddComponent<BurnEffect>();
            newFX.casterID = myID;
            newFX.damageAmount = burnDmg;
        }   
    }
}