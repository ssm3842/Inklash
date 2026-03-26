using UnityEngine;

public class FireUnit : Units
{
    protected override void ApplyBurnEffect(DamageableObject hitTarget)
    {
        string myID = gameObject.GetInstanceID().ToString();
        BurnEffect effect = hitTarget.GetComponent<BurnEffect>();

        float burnDmg =  HasBuff("Burn") ? 4f : 2f;

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