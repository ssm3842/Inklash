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
        float explosionDamage = 1.0f;
        if (HasBuff("Marker"))
        {
            target.gameObject.GetComponent<BuffController>().GetBuff(new BuffMarking(3f)); 
        }
        if (HasBuff("Weaker"))
        {
            target.gameObject.GetComponent<BuffController>().GetBuff(new BuffWeaken(3f)); 
        }
        if (HasBuff("Chiller"))
        {
            target.gameObject.GetComponent<BuffController>().GetBuff(new BuffChilling(3f)); 
        }
        if (HasBuff("Burn"))
        {
            target.gameObject.AddComponent<BurnEffect>(); 
        }
        if (HasBuff("Explosion"))
        {
             StartCoroutine(target.gameObject.GetComponent<Units>().TakeDamage(explosionDamage,amount+0.5f));
        }

         if(HasBuff("Poison"))
        {
            PoisonEffect effect = target.gameObject.GetComponent<PoisonEffect>();
            if (effect == null)
            {
                effect = target.gameObject.AddComponent<PoisonEffect>();
            }
            effect.AddStack();
        }
    }

    protected void SetDepthSorting(GameObject obj, float yPosition)
    {
        int order = -Mathf.CeilToInt((yPosition - 0.3f) * 100f);
        foreach (var r in obj.GetComponentsInChildren<Renderer>())
            r.sortingOrder = order;
    }
}