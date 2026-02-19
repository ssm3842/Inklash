public class BuffCold : Buffs
{
    public BuffCold() { this.buffName = "Cold"; this.remainTime = -1; }

    public override void OnGetBuff(DamageableObject owner)
    {
        Units unit = owner as Units;
        if (unit != null) unit.isColdAttack = true; 
    }
}