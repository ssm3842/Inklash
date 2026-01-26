using System.Collections;
using System.Linq;
using UnityEngine;

public class Units : DamageableObject
{
    protected bool isAttacking = false;

    [SerializeField] protected bool canAttack;
    [SerializeField] protected float canAttackTimer;

    private bool isDead = false;       
    public bool IsDead => isDead;
    private SpriteRenderer SR;        
    private Collider2D COL;           
    [SerializeField] private float deathDuration = 0.3f; 

    Rigidbody2D RB;
    Animator ANI;

    protected DamageableObject target;

    private bool isKnockedBack = false;
    private Coroutine currentKnockbackRoutine = null;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANI = GetComponent<Animator>();

        SR = GetComponent<SpriteRenderer>();
        COL = GetComponent<Collider2D>();
    }

    override public void Init(bool isplayers, UnitStats stats)
    {
        base.Init(isplayers, stats);

        isDead = false; 
        isKnockedBack = false;

        canAttack = true;
        canAttackTimer = 0f;

        GetComponent<Renderer>().sortingOrder = -Mathf.CeilToInt((transform.position.y - 0.3f) * 100f);
    }

    void Update()
    {
        //사망 시 행동 불능.
        if (isDead) return;

        if (isKnockedBack)
        {
            if (RB != null) RB.linearVelocity = Vector2.zero;
            return;
        }
        //디버프로 인한 행동불능이 있는지 검사.
        if(buffController.HaveDisruptEffect())
        {
            //기존 효과를 함수로 이전.
            OnDisruptEffect();

            return;
        }
        else
        {
            ANI.speed = 1f;
        }

        ANI.speed = statController.GetStat(StatType.ATKSPD);

        if (target != null)
        {
            Units targetUnit = target.GetComponent<Units>();
            if (targetUnit != null && targetUnit.IsDead) target = null;    
            else 
            {
                float distance = Mathf.Abs(target.transform.position.x - transform.position.x);
                
                float myRange = statController.GetStat(StatType.RANGE);


                if (distance > myRange + 0.1f) 
                {
                    target = null;      
                    isAttacking = false; 
                }
            }
        }
        

        Move();

        if (!target) //타겟이 없을 때만 새로 검사.
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(statController.GetStat(StatType.RANGE), 0.6f), 0f, isPlayers ? Vector3.right : Vector3.left, 0f);
            //검사된 오브젝트들을 필터링 및 정렬.
            hits = hits 
                .Where(hit => hit.collider != null && hit.collider.CompareTag("Units") && hit.collider.GetComponent<DamageableObject>().isPlayers != isPlayers) //상대 유닛만 검사 포함.
                .OrderBy(hit => Vector3.Distance(hit.transform.position, transform.position)) //오름차순으로 정렬(가까운 오브젝트가 제일 앞에 옴)
                .ToArray();

            if (hits.Length <= 0) target = null; //자신만 감지된 경우 타겟 없음.
            else target = hits[0].collider.GetComponent<DamageableObject>();
        }

        //목표가 없고 공격 모션이 끝나면 이동.
        if (!target && !isAttacking) RB.linearVelocityX = isPlayers ? statController.GetStat(StatType.SPD) : -statController.GetStat(StatType.SPD);
        //목표가 있거나 공격 모션이 재생중이면 이동 불가.
        else RB.linearVelocityX = 0f;
        ANI.SetBool("IsMoving", RB.linearVelocity.magnitude > 0f);

        //공격이 가능하고 타겟이 있다면 애니메이션 재생으로 공격 실행.
        if (canAttack && target != null) 
        {
            canAttack = false;
            ANI.SetTrigger("Attacked");
        }

        if (canAttackTimer >= statController.GetStat(StatType.ATKTerm) && !canAttack) canAttack = true; //ATKTerm 만큼의 초마다 공격 가능 상태가 됨.
        else if (canAttackTimer >= statController.GetStat(StatType.ATKTerm) && canAttack) { }
        else canAttackTimer += Time.deltaTime * statController.GetStat(StatType.ATKSPD); //공격 속도만큼 빠르게 채워짐.
    }

    public virtual void OnDisruptEffect()
    {
        //애니메이션 초기화.
            ANI.speed = 0f;
            ANI.SetTrigger("Disrupted");

            //공격 초기화
            target = null;
            isAttacking = false;
            canAttackTimer = 0f;
            canAttack = false;

            //이동속도 0
            RB.linearVelocityX = 0f;
    }

    public virtual void Move()
    {
        if (!target && !isAttacking) RB.linearVelocityX = isPlayers ? statController.GetStat(StatType.SPD) : -statController.GetStat(StatType.SPD); //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        ANI.SetBool("IsMoving", RB.linearVelocity.magnitude > 0f);

    }

    override public IEnumerator TakeDamage(float amount, float delayTime = 0f) //delayTime이 있다면 지연된 시간 후에 데미지.
    {
        if (isDead) yield break;

        yield return new WaitForSeconds(delayTime);
        if (statController.GetCurHp() <= amount) Die(); //남은 체력보다 데미지가 크면 오브젝트 파괴.
        else statController.ChangeCurHp(amount); //아니면 체력 계산.
    }

    public IEnumerator TakeDamage(float amount, bool isKnockback, float distance, float duration, float delayTime = 0f)
    {
        // 1. 기본 데미지 처리 (위의 함수 재사용)
        yield return StartCoroutine(this.TakeDamage(amount, delayTime));

        // 2. 살아있고, 넉백 공격이라면 실행
        if (!isDead && statController.GetCurHp() > 0 && isKnockback)
        {
            ApplyKnockback(distance, duration);
        }
    }

    public virtual void _AttackEnemy() //공격은 애니메이션에서 진행.
    {
        Units targetUnit = target?.GetComponent<Units>();
        float damage = statController.GetStat(StatType.ATK);

        if (targetUnit != null)
        {
            // ★ 핵심: 내 몸에 'KnockbackAttacker' 컴포넌트가 있는지 확인
            KnockBack kb = GetComponent<KnockBack>();

            if (kb != null)
            {
                // 넉백 컴포넌트가 있다면, 거기에 설정된 거리와 시간을 전달
                target.StartCoroutine(targetUnit.TakeDamage(damage, true, kb.pushDistance, kb.pushDuration));
            }
            else
            {
                // 넉백 컴포넌트가 없으면 일반 공격 (false)
                target.StartCoroutine(targetUnit.TakeDamage(damage, false, 0, 0));
            }
        }
        else if (target != null) // 건물이거나 Units 컴포넌트가 없는 경우
        {
            target.StartCoroutine(target.TakeDamage(damage, 0f));
        }
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

    //사망모션 임의 구현
    private void Die()
    {
        isDead = true; 
        isKnockedBack = false;

        if (RB != null) RB.linearVelocity = Vector2.zero;
        if (ANI != null) ANI.enabled = false; 

        if (COL != null) COL.enabled = false;

        StartCoroutine(DeathEffectCoroutine());
    }

    private IEnumerator DeathEffectCoroutine()
    {
        float timer = 0f;
        
        Vector3 initialScale = transform.localScale;
        Color initialColor = (SR != null) ? SR.color : Color.white;

        while (timer < deathDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / deathDuration; 

            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, progress);

            /* 투명하게
            if (SR != null)
            {
                Color newColor = initialColor;
                newColor.a = Mathf.Lerp(initialColor.a, 0f, progress);
                SR.color = newColor;
            }
            */
            yield return null; 
        }

        Destroy(this.gameObject);
    }

    void ApplyKnockback(float distance, float duration)
    {
        if (currentKnockbackRoutine != null)
        {
            StopCoroutine(currentKnockbackRoutine);
        }
        currentKnockbackRoutine = StartCoroutine(KnockbackRoutine(distance, duration));
    }

    IEnumerator KnockbackRoutine(float distance, float duration)
    {
        isKnockedBack = true;
        
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        // 아군이면 왼쪽으로, 적이면 오른쪽으로 밀림
        Vector3 pushDir = isPlayers ? Vector3.left : Vector3.right;
        Vector3 targetPos = startPos + (pushDir * distance);

        while (elapsed < duration)
        {
            if (isDead) break;

            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isDead) transform.position = targetPos;
        isKnockedBack = false;
        currentKnockbackRoutine = null;
    }
}