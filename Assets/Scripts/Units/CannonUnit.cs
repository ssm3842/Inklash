using System.Collections; // [필수] IEnumerator 사용을 위해 필요합니다
using UnityEngine;

public class CannonUnit : Units
{
    [SerializeField] GameObject shellPrefab;

    public override void _AttackEnemy()
    {
        if (!target) 
        {
            return;
        }

        GameObject newShell = Instantiate(shellPrefab, transform.position, Quaternion.identity);
        newShell.GetComponent<CannonShell>().targetPos = target.transform.position;

        HitEffectPacket finalPacket = GetFinalHitPacket(target);

        StartCoroutine(ApplyDelayedEffect(target, finalPacket, 0.2f));

        target.StartCoroutine(target.TakeDamage(statController.GetStat(StatType.ATK), 0.2f));
    }

    private IEnumerator ApplyDelayedEffect(DamageableObject targetObj, HitEffectPacket packet, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetObj != null)
        {
            var handler = targetObj.GetComponent<UnitStatusHandler>();
            if (handler != null && packet != null)
            {
                handler.ProcessHitEffects(packet);
            }
        }
    }
}