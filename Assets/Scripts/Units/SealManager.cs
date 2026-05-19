using System.Collections.Generic;
using UnityEngine;
public static class SealManager
{

    public static void ApplySeals(GameObject unitObj, List<SealType> seals)
    {
        if (unitObj == null || seals.Count == 0) return;
        IBuffable bc = unitObj.GetComponent<IBuffable>();
        if (bc == null) return;

        foreach(SealType type in seals)
        {
            switch(type)
            {
                case SealType.Ignite:
                    bc.GetBuff(new BuffBurn());
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
                case SealType.Weaken:
                    bc.GetBuff(new BuffWeaker()); 
                    break;
                case SealType.Chill:
                    bc.GetBuff(new BuffChiller());
                    break;
                case SealType.Purity:
                    bc.GetBuff(new BuffPurity());  
                    break;
                case SealType.Copy:
                    bc.GetBuff(new BuffCopy());
                    break;
            }
        }
    }

    // 인장 추가
    public static void AddSealToCard(CardContent card, SealType newSeal)
    {
        if (card == null) return;

        card.seals.Add(newSeal);
    }

    public static bool IsHaveSomeSeal(CardContent card, SealType someSeal)
    {
        return card.seals.Contains(someSeal);
    }

    // 인장 제거
    public static void RemoveSealFromCard(CardContent card, SealType targetSeal)
    {
        if (card == null) return;

        card.seals.Remove(targetSeal);
    }
}