using UnityEngine;
using System.Collections;

public class BurnEffect : MonoBehaviour
{
    public string casterID;
    public float damageAmount = 2f;
    private float elapsed = 0f;

    public void ResetTimer() => elapsed = 0f;

    IEnumerator Start()
    {
        DamageableObject target = GetComponent<DamageableObject>();
        while (elapsed <= 3f)
        {
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
            if (target != null) StartCoroutine(target.TakeDamage(damageAmount, 0f));
        }
        Destroy(this);
    }
}