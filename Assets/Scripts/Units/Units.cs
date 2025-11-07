using UnityEditor.Animations;
using UnityEngine;

public class Units : Entity
{
    [SerializeField] float ATK;
    float MoveSPD;
    float AtkSPD;
    float Range;

    bool isAttacking = false;
    bool isTriggered = false;

    [SerializeField] bool canAttack;
    [SerializeField] float canAttackTimer;

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

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(Range, 0.6f), 0f, isPlayers ? Vector3.right : Vector3.left, 0f);

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

        if (canAttackTimer >= 3f && !canAttack) canAttack = true;//2초마다 공격 가능 상태가 됨.
        else if (canAttackTimer >= 3f && canAttack) { }
        else canAttackTimer += Time.deltaTime * AtkSPD; //공격 속도만큼 빠르게 채워짐.

        //Attack 애니메이션이 재생중이면 true
        bool isAttackPlayed = ANI.GetCurrentAnimatorStateInfo(0).IsName("Attack") && ANI.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f;
        if (canAttack && target != null && !isTriggered && !isAttackPlayed) //공격 가능한 상태에 애니메이션 재생.
        {
            ANI.SetTrigger("Attacked");
            isTriggered = true;
            Debug.Log("!!");
        }
    }

    public void _AttackEnemy() //공격은 애니메이션에서 진행.
    {
        target?.TakeDamage(ATK);
        canAttackTimer = 0f;
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

    public void _OnAttackStart()
    {
        isTriggered = false;
        canAttack = false;
        isAttacking = true;
    }
    public void _OnAttackEnd()
    {
        isAttacking = false;
    }
}