using UnityEngine;

public class BowUnit : Units
{
    [SerializeField] GameObject arrowPrefab;
    public override void _AttackEnemy()
    {
        GameObject newArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        newArrow.GetComponent<Arrow>().targetPos = target.transform.position;

        target?.StartCoroutine(target.TakeDamage(ATK, 1f));
        canAttackTimer = 0f;
    }
}
