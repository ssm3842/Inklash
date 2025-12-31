using UnityEngine;

public class BowUnit : Units
{
    [SerializeField] GameObject arrowPrefab;
    public override void _AttackEnemy()
    {
        if (!target) //공격을 실행할 때 타겟이 유효하지 않으면 공격 무효화.
        {
            return;
        }
        
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        newArrow.transform.SetParent(RunManager.Inst.battleManager.cardUseManager.transform);
        newArrow.GetComponent<Arrow>().targetPos = target.transform.position;

        target?.StartCoroutine(target.TakeDamage(statController.GetStat(StatType.ATK), 1f));
    }
}
