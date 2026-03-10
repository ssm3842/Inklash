public class BuffWeaker : Buffs{
    public BuffWeaker() 
    { 
        this.buffName = "Weaker"; 
        this.remainTime = -1;;
    }

}

public class BuffWeaken : Buffs
{
    public BuffWeaken(float remainTime = 3.0f) 
    { 
        this.buffName = "Weaken"; 
        this.amount = 0.25f;
        this.remainTime = remainTime;
    }

    protected override void ApplyUnit(Units unit) 
    {
        statController.ControlBonusStat(StatType.ATK, -amount);
    }

    public override void OnBuffEnd()
    {
        statController.ControlBonusStat(StatType.ATK, amount);
    }

}