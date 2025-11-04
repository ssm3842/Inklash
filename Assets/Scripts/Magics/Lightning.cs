using UnityEngine;

public class Lightning : MonoBehaviour
{
    void Start()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1);

        foreach (Collider2D enemy in enemies)
        {
            // enemy.gameObject.GetComponent<Units>().TakeDamage()
        }
    }
}
