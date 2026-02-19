public class BuffBurn : Buffs
{
    public BuffBurn() { this.buffName = "Burn"; this.remainTime = -1; }

    public override void OnGetBuff(DamageableObject owner)
    {
        Units unit = owner as Units;
        if (unit != null) unit.isBurnAttack = true;
    }
}