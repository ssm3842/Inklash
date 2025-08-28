using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class Units : MonoBehaviour
{
    private float MaxHP = 10f;
    private float CurHP;
    private float ATK = 6f;
    private float MoveSPD = 0.5f;
    private float AtkSPD = 1f;

    private bool canAttack;
    private float canAttackTimer;

    public bool isPlayers;

    Rigidbody2D RB;

    Units target;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public void Init(bool players)
    {
        CurHP = MaxHP;
        canAttack = true;
        canAttackTimer = 0f;

        isPlayers = players;
    }

    void Update()
    {
        if (!target) RB.linearVelocityX = isPlayers ? MoveSPD : -MoveSPD; //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, isPlayers ? Vector3.right : Vector3.left, 0.3f); //레이캐스트로 타겟 검사
        if (hits.Length == 0) target = null;
        else
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Units") && (hit.collider.GetComponent<Units>().isPlayers != isPlayers))
                {
                    target = hit.collider.GetComponent<Units>();
                    break;
                }
            }
        }

        if (canAttackTimer >= (1 / AtkSPD))
        {
            canAttack = true;
        }
        else canAttackTimer += Time.deltaTime;

        if (canAttack && target) AttackEnemy(target);
    }

    void AttackEnemy(Units enemy)
    {
        enemy.TakeDamage(ATK);
        canAttackTimer = 0f;
        canAttack = false;
    }

    public void TakeDamage(float amount)
    {
        if (CurHP < amount) Destroy(this.gameObject);
        else
        {
            CurHP -= amount;
            Debug.Log(CurHP);
        }
    }
}
