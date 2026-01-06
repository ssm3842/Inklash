using UnityEngine;

public class CannonUnit : Units
{
    [SerializeField] GameObject shellPrefab;
    public override void _AttackEnemy()
    {
        if (!target) //공격을 실행할 때 타겟이 유효하지 않으면 공격 무효화.
        {
            return;
        }
        
    
        GameObject newShell = Instantiate(shellPrefab, transform.position, Quaternion.identity);
        newShell.GetComponent<CannonShell>().targetPos = target.transform.position;

        target?.StartCoroutine(target.TakeDamage(statController.GetStat(StatType.ATK), 0.2f));
    }
}
