using UnityEngine;

public class KnockbackAttackEffect : AttackEffect
{
    [SerializeField] private float pushForce = 1.5f;

    public override void ApplyEffect(HitEffectPacket packet)
    {
        // 유닛의 방향을 고려하여 넉백 힘을 설정합니다.
        Units owner = GetComponent<Units>();
        Vector2 direction = owner.isPlayers ? Vector2.right : Vector2.left;
        packet.KnockbackForce = direction * pushForce;
    }
}