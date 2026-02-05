using System.Collections;
using UnityEngine;
public class BowUnit : Units
{
    [SerializeField] GameObject arrowPrefab;
    
    public override void _AttackEnemy()
    {
        if (!target) return;
        
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        newArrow.transform.SetParent(RunManager.Inst.battleManager.cardUseManager.transform);
        newArrow.GetComponent<Arrow>().targetPos = target.transform.position;

        HitEffectPacket finalPacket = GetFinalHitPacket(target);

        StartCoroutine(ApplyDelayedEffect(target, finalPacket, 1f));
        target.StartCoroutine(target.TakeDamage(statController.GetStat(StatType.ATK), 1f));
    }

    private IEnumerator ApplyDelayedEffect(DamageableObject targetObj, HitEffectPacket packet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetObj != null && !targetObj.GetComponent<Units>().isDead)
        {
            var handler = targetObj.GetComponent<UnitStatusHandler>();
            if (handler != null) handler.ProcessHitEffects(packet);
        }
    }
}
