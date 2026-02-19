public class BuffPierce : Buffs
{
    public BuffPierce() 
    { 
        this.buffName = "Pierce"; 
        this.remainTime = -1; 
    }

    public override void OnGetBuff(DamageableObject owner)
    {
        Units unit = owner as Units;
        if (unit != null)
        {
            unit.isPierceAttack = true; 
        }
    }
}