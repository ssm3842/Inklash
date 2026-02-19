public class BuffDoubleAttack : Buffs
{
    public BuffDoubleAttack()
    {
        this.buffName = "DoubleAttack";
        this.remainTime = -1;
    }

    public override void OnGetBuff(DamageableObject owner)
    {
        Units unit = owner as Units;
        if (unit != null)
        {
            unit.isDoubleAttack = true; 
        }
    }
}