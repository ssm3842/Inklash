using UnityEngine;

public class UnitStatusHandler : MonoBehaviour
{
    private Units unit;
    private int freezeHitCount = 0;

    void Awake() => unit = GetComponent<Units>();

    public void ProcessHitEffects(HitEffectPacket packet)
    {
        if (unit == null || unit.isDead) return;

        ApplyKnockback(packet.KnockbackForce);

        if (packet.IsFreezeAttack) 
            ApplyFreeze();
    }

    private void ApplyKnockback(Vector2 force)
    {
        if (force != Vector2.zero)
            unit.ChangeState(new KnockbackState(unit, force, 0.2f));
    }

    private void ApplyFreeze()
    {
        freezeHitCount++;
        Debug.Log($"{gameObject.name} 빙결 스택: {freezeHitCount}/3");

        if (freezeHitCount >= 3)
        {
            // 3회 충족 시 본인의 BuffController에 빙결 버프 추가
            if (unit.buffController != null)
            {
                unit.buffController.GetBuff(new BuffFreeze(2f)); // 2초간 빙결
                freezeHitCount = 0; // 카운트 초기화
            }
        }
    }
}