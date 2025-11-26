using System.Collections;
using System.Linq;
using UnityEngine;

public class Units : DamageableObject
{
    bool isAttacking = false;

    [SerializeField] protected bool canAttack;
    [SerializeField] protected float canAttackTimer;

    Rigidbody2D RB;
    Animator ANI;

    protected DamageableObject target;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANI = GetComponent<Animator>();
    }

    override public void Init(bool isplayers, UnitStats stats)
    {
        base.Init(isplayers, stats);

        canAttack = true;
        canAttackTimer = 0f;

        GetComponent<Renderer>().sortingOrder = -Mathf.CeilToInt((transform.position.y - 0.3f) * 100f);

    }

    void Update()
    {
        if (!target && !isAttacking) RB.linearVelocityX = isPlayers ? statController.GetStat(StatType.SPD) : -statController.GetStat(StatType.SPD); //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        ANI.SetBool("IsMoving", RB.linearVelocity.magnitude > 0f);

        if (!target) //타겟이 없을 때만 새로 검사.
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(statController.GetStat(StatType.RANGE), 0.6f), 0f, isPlayers ? Vector3.right : Vector3.left, 0f);
            hits = hits //검사된 오브젝트들을 필터링 및 정렬.
                .Where(hit => hit.collider != null && hit.collider.CompareTag("Units") && hit.collider.GetComponent<DamageableObject>().isPlayers != isPlayers) //상대 유닛만 검사 포함.
                .OrderBy(hit => Vector3.Distance(hit.transform.position, transform.position)) //오름차순으로 정렬(가까운 오브젝트가 제일 앞에 옴)
                .ToArray();

            if (hits.Length <= 0) target = null; //자신만 감지된 경우 타겟 없음.
            else target = hits[0].collider.GetComponent<DamageableObject>();
        }

        //Attack 애니메이션이 재생중이면 true
        if (canAttack && target != null) //공격 가능한 상태에 애니메이션 재생.
        {
            canAttack = false;
            ANI.SetTrigger("Attacked");
        }

        if (canAttackTimer >= 2f && !canAttack) canAttack = true;//2초마다 공격 가능 상태가 됨.
        else if (canAttackTimer >= 2f && canAttack) { }
        else canAttackTimer += Time.deltaTime * statController.GetStat(StatType.ATKSPD); //공격 속도만큼 빠르게 채워짐.
    }

    override public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        yield return new WaitForSeconds(delayTime);
        if (statController.GetCurHp() <= amount) Destroy(gameObject); //남은 체력보다 데미지가 크면 오브젝트 파괴.
        else statController.ChangeCurHp(amount); //아니면 체력 계산.
    }

    public virtual void _AttackEnemy() //공격은 애니메이션에서 진행.
    {
        target?.StartCoroutine(target.TakeDamage(statController.GetStat(StatType.ATK), 0f));
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