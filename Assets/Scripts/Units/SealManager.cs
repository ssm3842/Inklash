using Unity.VisualScripting;
using UnityEngine;
public static class SealManager
{
    public static void ApplySeals(GameObject unitObj, SealType seals)
    {
        if (unitObj == null || seals == SealType.None) return;
        BuffController bc = unitObj.GetComponent<BuffController>();
        if (bc == null) return;

        // 비트 플래그 체크 (중첩 가능)
        if ((seals & SealType.Burn) != 0) bc.GetBuff(new BuffBurn());
        if ((seals & SealType.Cold) != 0) bc.GetBuff(new BuffCold());
        if ((seals & SealType.DoubleAttack) != 0) bc.GetBuff(new BuffDoubleAttack());
        if ((seals & SealType.KnockBack) != 0) bc.GetBuff(new BuffKnockback());
        if ((seals & SealType.Pierce) != 0) bc.GetBuff(new BuffPierce());
        if ((seals & SealType.Discard) != 0) bc.GetBuff(new BuffDiscard());
        if ((seals & SealType.StartCost) != 0) bc.GetBuff(new BuffStartCost());
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