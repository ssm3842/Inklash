using UnityEngine;
public class BuffDiscard : Buffs
{
    public BuffDiscard()
    {
        this.buffName = "DiscardSeal";
        this.remainTime = -1;
    }

    public override void OnGetBuff(GameObject owner)
    {
        CardManager cardManager = UnityEngine.Object.FindAnyObjectByType<CardManager>();
        if (cardManager != null)
        {
            cardManager.DiscardCard();
        }
    }
}