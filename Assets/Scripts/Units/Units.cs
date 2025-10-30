using UnityEngine;

public class Units : Entity
{
    [SerializeField] private float ATK;
    private float MoveSPD;
    private float AtkSPD;
    private float Range;

    private bool isAttacking = false;

    [SerializeField] private bool canAttack;
    [SerializeField]private float canAttackTimer;

    Rigidbody2D RB;
    Animator ANI;

    Entity target;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANI = GetComponent<Animator>();
    }

    override public void Init(bool isplayers, CardContent card)
    {
        this.ATK = card.stats.atk;
        this.MoveSPD = card.stats.spd;
        this.AtkSPD = card.stats.atkSpd;
        ANI.SetFloat("AtkSpd", AtkSPD);
        this.Range = card.stats.range;

        canAttack = true;
        canAttackTimer = 0f;
        base.Init(isplayers, card);
        
        isPlayers = isplayers;
    }

    void Update()
    {
        if (!target && !isAttacking) RB.linearVelocityX = isPlayers ? MoveSPD : -MoveSPD; //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        ANI.SetBool("IsMoving", RB.linearVelocity.magnitude > 0f);

        // RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, isPlayers ? Vector3.right : Vector3.left, Range); //레이캐스트로 타겟 검사
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Range, 0.6f), 0f, isPlayers ? Vector3.right : Vector3.left, 0f);
        Debug.Log(hits.Length);

        if (hits.Length <= 1) //TODO: 수정필요. 루프가 끝나면 적이 없는 것이므로 그 때 타켓 초기화 진행.
            target = null;
        else
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Units") && (hit.collider.GetComponent<Entity>().isPlayers != isPlayers)) //팀이 다른 유닛 발견 시 까지 검사.
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

        if (canAttack && target)
        {
            ANI.SetTrigger("Attacked");
            isAttacking = true;
            AttackEnemy(target);
        }
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
            // Debug.Log(CurHP);
        }
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(transform.position, new Vector3(Range, 0.6f, 0f));
}
}