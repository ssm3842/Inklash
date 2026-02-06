public class ColdAttackEffect : AttackEffect
{
    public override void ApplyEffect(HitEffectPacket packet)
    {
        packet.IsFreezeAttack = true;
    }
}