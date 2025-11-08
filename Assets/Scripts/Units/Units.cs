using System.Collections;
using UnityEngine;

public class Units : Entity
{
    [SerializeField] protected float ATK;
    float MoveSPD;
    float AtkSPD;
    float Range;

    bool isAttacking = false;

    [SerializeField] protected bool canAttack;
    [SerializeField] protected float canAttackTimer;

    Rigidbody2D RB;
    Animator ANI;

    protected Entity target;

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
        //Attack 애니메이션이 재생중이면 true
        if (canAttack && target != null) //공격 가능한 상태에 애니메이션 재생.
        {
            canAttack = false;
            ANI.SetTrigger("Attacked");
        }

        if (canAttackTimer >= 3f && !canAttack) canAttack = true;//2초마다 공격 가능 상태가 됨.
        else if (canAttackTimer >= 3f && canAttack) { }
        else canAttackTimer += Time.deltaTime * AtkSPD; //공격 속도만큼 빠르게 채워짐.
    }

    override public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        Debug.Log("dfdfd");
        yield return new WaitForSeconds(delayTime);
        if (CurHP <= amount) Destroy(this.gameObject);
        else
        {
            CurHP -= amount;
            // Debug.Log(CurHP);
        }
    }

    public virtual void _AttackEnemy() //공격은 애니메이션에서 진행.
    {
        target?.StartCoroutine(TakeDamage(ATK, 0f));
    }

    public void _OnAttackStart() //공격 애니메이션 시작 시 관련 변수를 초기화.
    {
        canAttackTimer = 0f;
        canAttack = false;
        isAttacking = true;
    }
    public void _OnAttackEnd() //공격 애니메이션 종료 시 이동할 수 있도록 변수 초기화.
    {
        isAttacking = false;
    }
}