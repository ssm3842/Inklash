public class BuffUltimate : Buffs
{
    public BuffUltimate() 
    { 
        this.buffName = "Ultimate"; 
        this.remainTime = -1; 
    }
    
    protected override void ApplyUnit(Units unit)
    {
        statController.ControlBonusStat(StatType.ATK, 1f);
        statController.ControlBonusStat(StatType.MAX_HP, 1f);
        statController.ControlBonusStat(StatType.SPD, 0.2f);
        statController.ControlBonusStat(StatType.ATKSPD, 0.2f);
    }

    protected override void ApplySpell(SpellBase spell)
    {

    }
}