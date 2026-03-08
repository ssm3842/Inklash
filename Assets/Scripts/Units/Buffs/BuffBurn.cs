public class BuffBurn : Buffs
{
    public BuffBurn() { this.buffName = "Burn"; this.remainTime = -1; }
    
    protected override void ApplyUnit(Units unit)
    {
        unit.isBurnAttack = true;
    }
}