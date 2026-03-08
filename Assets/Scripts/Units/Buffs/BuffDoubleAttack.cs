using UnityEngine;

public class BuffDoubleAttack : Buffs
{
    public BuffDoubleAttack()
    {
        this.buffName = "DoubleAttack";
        this.remainTime = -1;
    }

    public override void OnGetBuff(DamageableObject owner)
    {
        
    }
}