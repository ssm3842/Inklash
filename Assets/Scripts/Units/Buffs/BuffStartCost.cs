using UnityEngine;

public class BuffStartCost : Buffs
{
    private int refundAmount = 2;

    public BuffStartCost(int amount = 2)
    {
        this.buffName = "StartCost";
        this.remainTime = -1; 
        this.refundAmount = amount;
    }

    public override void OnGetBuff(GameObject owner)
    {
        CostManager costManager = UnityEngine.Object.FindAnyObjectByType<CostManager>();
        if (costManager != null)
        {
            costManager.AddCost(refundAmount);
        }
    }
}