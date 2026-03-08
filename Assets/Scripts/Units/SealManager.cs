using System.Collections.Generic;
using UnityEngine;
public static class SealManager
{

    public static void ApplySeals(GameObject unitObj, List<SealType> seals)
    {
        if (unitObj == null || seals.Count == 0) return;
        BuffController bc = unitObj.GetComponent<BuffController>();
        if (bc == null) return;

        foreach(SealType type in seals)
        {
            switch(type)
            {
                case SealType.Ignite:
                    bc.GetBuff(new BuffBurn());
                    break;
                case SealType.Cold:
                    bc.GetBuff(new BuffCold());
                    break;
                case SealType.ExtraHit:
                    bc.GetBuff(new BuffDoubleAttack());
                    break;
                case SealType.KnockBack:
                    bc.GetBuff(new BuffKnockback());
                    break;
                case SealType.Pierce:
                    bc.GetBuff(new BuffPierce());
                    break;
                case SealType.Ultimate:
                    bc.GetBuff(new BuffUltimate());
                    break;
                case SealType.Split:
                    bc.GetBuff(new BuffSplit());
                    break;   
                case SealType.Explosion:
                    bc.GetBuff(new BuffExplosion());
                    break;
                case SealType.Mark:
                    bc.GetBuff(new BuffMarker());
                    break;
                case SealType.Weak:
                    bc.GetBuff(new BuffWeaker()); 
                    break;
                case SealType.Chill:
                    bc.GetBuff(new BuffChiller());
                    break; 
                
            }
        }
    }

    private static void AddOnHitTag(DamageableObject unit, string tagName)
    {
        if (!unit.onHitBuffTags.Contains(tagName))
        {
            unit.onHitBuffTags.Add(tagName);
        }
    }

    public static int GetSealCount(CardContent card)
    {
        // 몇개의 인장이 활성화 되었는지 검사
        int count = 0;
        foreach (SealType type in System.Enum.GetValues(typeof(SealType)))
        {
            if (type != SealType.None && (card.seals & type) == type)
            {
                count++;
            }
        }
        return count;
    }

    // 인장 추가
    public static void AddSealToCard(CardContent card, SealType newSeal)
    {
        if (card == null) return;

        card.seals |= newSeal;
    }

    // 인장 제거
    public static void RemoveSealFromCard(CardContent card, SealType targetSeal)
    {
        if (card == null) return;

        card.seals &= ~targetSeal;
    }
}