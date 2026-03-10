public class BuffChiller : Buffs
{
    public BuffChiller() 
    { 
        this.buffName = "Chiller"; 
        this.remainTime = -1f; 
    }
}

public class BuffChilling : Buffs
{
    public BuffChilling(float remainTime = 3.0f) 
    { 
        this.buffName = "Chilling"; 
        this.remainTime = remainTime; 
        this.amount = 0.25f;
    }

    protected override void ApplyUnit(Units unit) 
    {
        statController.ControlBonusStat(StatType.ATKSPD, -amount);
    }

    public override void OnBuffEnd()
    {
        statController.ControlBonusStat(StatType.ATKSPD, amount);
    }
}