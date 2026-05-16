using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

public class SpellBase : MonoBehaviour , IBuffable
{   
    public List<Buffs> buffList = new List<Buffs>();
    protected float finalAmount;
    protected float finalRange;

    [Header("Depth Scale")]
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public float scaleYMin = -2f;
    public float scaleYMax = -0.4f;

    public void ProcessSpell(float amount, float range, float castXPosition)
    {
        finalAmount = amount;
        finalRange = range;

        BeforeCast();

        CastSpell(finalAmount, finalRange, castXPosition);

    }

    public virtual void CastSpell(float amount, float range, float castXPosition) { }

    public void _OnAnimationEnd()
    {
        Destroy(gameObject);
    }
    public void GetBuff(Buffs newBuff)
    {
        if (buffList.Exists(b => b.buffName == newBuff.buffName)) return;
        buffList.Add(newBuff);
        newBuff.OnGetBuff(this.gameObject);
    }

    public bool HasBuff(string buffName)
    {
        return buffList.Exists(b => b.buffName == buffName);
    }

    public void BeforeCast()
    {
        if (buffList == null) return; 

        if (HasBuff("Split"))
        {
            finalAmount *= 0.5f;
            finalRange *= 0.5f;
        }

        if(HasBuff("Ultimate"))
        {
            finalAmount *= 2.0f;
            finalRange *=2.0f;
        }        
    }

    public void PerformHit(Collider2D target, float amount = 0)
    {
        if (target == null) return;

        var damageable = target.gameObject.GetComponent<DamageableObject>();
        if (damageable == null) return;

        float explosionDamage = target.gameObject.GetComponent<StatController>().GetStat(StatType.ATK);
        
        var bc = target.gameObject.GetComponent<BuffController>();
        if (bc != null)
        {
            if (HasBuff("Marker"))  bc.GetBuff(new BuffMarking(3f));
            if (HasBuff("Weaker"))  bc.GetBuff(new BuffWeaken(3f));
            if (HasBuff("Chiller")) bc.GetBuff(new BuffChilling(3f));
        }
        
        if (HasBuff("Burn"))
        {
            BurnEffect existing = target.gameObject.GetComponent<BurnEffect>();
            if (existing != null) existing.ResetTimer();
            else target.gameObject.AddComponent<BurnEffect>();
        }
        
        if (HasBuff("Explosion"))
        {
            StartCoroutine(damageable.TakeDamage(explosionDamage, amount + 0.5f));
        }

        if (HasBuff("Poison"))
        {
            PoisonEffect effect = target.gameObject.GetComponent<PoisonEffect>();
            if (effect == null) effect = target.gameObject.AddComponent<PoisonEffect>();
            effect.AddStack();
        }
    }

    protected void SetDepthSorting(GameObject obj, float yPosition)
    {
        int order = -Mathf.CeilToInt((yPosition - 0.3f) * 100f);
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
            r.sortingOrder = order;
    }

    protected void ApplyDepthScale(GameObject obj, float y)
    {
        float t = Mathf.InverseLerp(scaleYMax, scaleYMin, y);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        obj.transform.localScale *= scale;
    }
}