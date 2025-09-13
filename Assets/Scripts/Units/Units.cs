using TMPro;
using UnityEngine;

public class Units : Entity
{
    [SerializeField] private float ATK;
    private float MoveSPD;
    private float AtkSPD;
    private float Range;

    [SerializeField] private bool canAttack;
    [SerializeField]private float canAttackTimer;

    Rigidbody2D RB;

    Entity target;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    override public void Init(bool isplayers, CardContent card)
    {
        this.ATK = card.stats.atk;
        this.MoveSPD = card.stats.spd;
        this.AtkSPD = card.stats.atkSpd;
        this.Range = card.stats.range;

        canAttack = true;
        canAttackTimer = 0f;
        base.Init(isplayers, card);
        
        isPlayers = isplayers;
    }

    void Update()
    {
        if (!target) RB.linearVelocityX = isPlayers ? MoveSPD : -MoveSPD; //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, isPlayers ? Vector3.right : Vector3.left, Range); //레이캐스트로 타겟 검사
        if (hits.Length == 0) target = null;
        else
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Units") && (hit.collider.GetComponent<Entity>().isPlayers != isPlayers))
                {
                    target = hit.collider.GetComponent<Entity>();
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

    void AttackEnemy(Entity enemy)
    {
        enemy.TakeDamage(ATK);
        canAttackTimer = 0f;
        canAttack = false;
    }

    override public void TakeDamage(float amount)
    {
        if (CurHP <= amount) Destroy(this.gameObject);
        else
        {
            CurHP -= amount;
            Debug.Log(CurHP);
        }
    }
}