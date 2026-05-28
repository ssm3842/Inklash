using UnityEngine;
using System.Collections;

public class BurnEffect : MonoBehaviour
{
    public string casterID;
    public float damageAmount;
    private float elapsed = 0f;

    public void ResetTimer() => elapsed = 0f;
    
    IEnumerator Start()
    {
        DamageableObject target = GetComponent<DamageableObject>();
        damageAmount = target.GetComponent<StatController>().GetStat(StatType.MAX_HP) * 0.05f;
        if (target.gameObject.name.Contains("Base"))
        {
            damageAmount *= 0.1f; // 기지일 때는 초당 1%
        }
        while (elapsed <= 3f)
        {
            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;
            if (target != null) StartCoroutine(target.TakeDamage(damageAmount, 0f, true));
        }
        Destroy(this);
    }
}
