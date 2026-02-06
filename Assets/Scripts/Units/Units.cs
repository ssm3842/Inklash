using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IUnitState { void Enter(); void Execute(); void Exit(); }

[RequireComponent(typeof(UnitStatusHandler))]
public class Units : DamageableObject
{
    protected IUnitState currentState;
    public StatController StatControl => statController; 
    protected UnitStatusHandler statusHandler;

    protected DamageableObject target; 
    public DamageableObject Target { get => target; set => target = value; }

    public Rigidbody2D RB { get; private set; }
    public Animator ANI { get; private set; }
    protected Collider2D COL;
    public Action<DamageableObject> OnAttackPerformed;

    public bool isAttacking = false;
    public bool canAttack = true;
    public float canAttackTimer = 0f;
    public bool isDead = false;
    private Vector3 originalScale;

    // 내 몸에 붙은 특수 공격 부품들 (bool 변수 대신 사용)
    private List<AttackEffect> attackEffects = new List<AttackEffect>();

    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANI = GetComponent<Animator>();
        COL = GetComponent<Collider2D>();
        statusHandler = GetComponent<UnitStatusHandler>();
        originalScale = transform.localScale;
    }

    public override void Init(bool players, UnitStats stats)
    {
        base.Init(players, stats);
        isDead = false; canAttack = true; isAttacking = false;
        if (COL) COL.enabled = true;
        if (ANI) ANI.enabled = true;
        transform.localScale = originalScale;
        SetDefaultState();
    }

    protected virtual void SetDefaultState() => ChangeState(new MoveState(this));

    public HitEffectPacket GetFinalHitPacket(DamageableObject victim)
    {
        HitEffectPacket packet = CreateHitPacket(victim);

        var effects = GetComponents<AttackEffect>();
        foreach (var effect in effects)
        {
            effect.ApplyEffect(packet);
        }

        return packet;
    }
    public void ChangeState(IUnitState newState)
    {
        if (isDead) return;
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    protected virtual void Update()
    {
        if (isDead || StatControl == null) return;
        HandleDisruptStatus();
        currentState?.Execute();
        UpdateAttackTimer();
    }

    public override IEnumerator TakeDamage(float amount, float delayTime = 0f)
    {
        if (isDead) yield break;
        yield return new WaitForSeconds(delayTime);
        if (StatControl.GetCurHp() <= amount) Die();
        else StatControl.ChangeCurHp(amount);
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        if (RB) RB.linearVelocity = Vector2.zero;
        if (ANI) ANI.enabled = false;
        if (COL) COL.enabled = false;
        StartCoroutine(DeathEffect());
    }

    private IEnumerator DeathEffect()
    {
        float t = 0;
        Vector3 s = transform.localScale;
        while (t < 0.3f) { t += Time.deltaTime; transform.localScale = Vector3.Lerp(s, Vector3.zero, t / 0.3f); yield return null; }
        Destroy(gameObject);
    }

    private void HandleDisruptStatus()
    {
        if (buffController == null) return;
        bool hasDisrupt = buffController.HaveDisruptEffect();
        if (hasDisrupt && currentState is not DisruptedState) ChangeState(new DisruptedState(this));
        else if (!hasDisrupt && currentState is DisruptedState) SetDefaultState();
    }

    private void UpdateAttackTimer()
    {
        float term = StatControl.GetStat(StatType.ATKTerm);
        float spd = StatControl.GetStat(StatType.ATKSPD);
        if (canAttackTimer >= term) canAttack = true;
        else canAttackTimer += Time.deltaTime * spd;
    }

    public virtual void _AttackEnemy() 
    { 
        if (target == null) return;

        target.StartCoroutine(target.TakeDamage(StatControl.GetStat(StatType.ATK), 0f)); 

        HitEffectPacket packet = CreateHitPacket(target);

        attackEffects.Clear();
        GetComponents<AttackEffect>(attackEffects);
        foreach (var effect in attackEffects)
        {
            effect.ApplyEffect(packet);
        }

        var targetHandler = target.GetComponent<UnitStatusHandler>();
        if (targetHandler != null) targetHandler.ProcessHitEffects(packet);

        OnAttackPerformed?.Invoke(target); 
    }

    private HitEffectPacket CreateHitPacket(DamageableObject victim)
    {
        return new HitEffectPacket
        {
            // 방향에 따른 넉백 힘 설정
            KnockbackForce = Vector2.zero,
            IsFreezeAttack = false 
        };
    }

    public void _OnAttackStart() { canAttackTimer = 0f; canAttack = false; isAttacking = true; }
    public void _OnAttackEnd() { isAttacking = false; }
}

public class MoveState : IUnitState
{
    protected Units u;
    public MoveState(Units unit) => u = unit;
    public virtual void Enter() { if(u.ANI) u.ANI.SetBool("IsMoving", true); }
    
    public virtual void Execute()
    {
        // 1. 타겟이 없으면 탐색
        if (u.Target == null)
        {
            float searchRange = 10f; // 탐색 범위
            RaycastHit2D[] hits = Physics2D.BoxCastAll(u.transform.position, new Vector2(searchRange, 0.6f), 0f, u.isPlayers ? Vector3.right : Vector3.left, 0f);
            var validHit = hits.Where(h => h.collider != null && h.collider.CompareTag("Units"))
                               .Select(h => new { h, d = h.collider.GetComponent<DamageableObject>() })
                               .Where(x => x.d != null && x.d.isPlayers != u.isPlayers)
                               .OrderBy(x => Vector3.Distance(x.h.transform.position, u.transform.position)).FirstOrDefault();
            if (validHit != null) u.Target = validHit.d;
        }

        // 2. 타겟이 있고 '사거리 안'에 들어오면 공격 상태로 전환
        if (u.Target != null) 
        {
            float dist = Vector2.Distance(u.transform.position, u.Target.transform.position);
            float attackRange = u.StatControl.GetStat(StatType.RANGE);

            if (dist <= attackRange)
            {
                u.ChangeState(new AttackState(u));
                return;
            }
        }

        // 3. 사거리 밖이라면 계속 이동
        u.RB.linearVelocityX = u.isPlayers ? u.StatControl.GetStat(StatType.SPD) : -u.StatControl.GetStat(StatType.SPD);
    }
    public virtual void Exit() { if(u.RB) u.RB.linearVelocityX = 0f; if(u.ANI) u.ANI.SetBool("IsMoving", false); }
}

public class AttackState : IUnitState
{
    protected Units u;
    public AttackState(Units unit) => u = unit;
    public void Enter() { if(u.RB) u.RB.linearVelocityX = 0f; }
    
    public void Execute()
    {
        if (u.Target == null) { u.ChangeState(new MoveState(u)); return; }

        // 넉백 등으로 타겟이 사거리 밖으로 벗어났는지 체크
        float dist = Vector2.Distance(u.transform.position, u.Target.transform.position);
        float attackRange = u.StatControl.GetStat(StatType.RANGE);

        if (dist > attackRange && !u.isAttacking) // 공격 중이 아닐 때만 추격 시작
        {
            u.ChangeState(new MoveState(u));
            return;
        }

        if (u.canAttack && u.Target != null) u.ANI.SetTrigger("Attacked");
    }
    public void Exit() { }
}

public class DisruptedState : IUnitState
{
    protected Units u;
    public DisruptedState(Units unit) => u = unit;
    public void Enter() { if(u.ANI) u.ANI.speed = 0f; if(u.RB) u.RB.linearVelocityX = 0f; u.isAttacking = false; }
    public void Execute() { }
    public void Exit() { if(u.ANI) u.ANI.speed = 1f; }
}

public class KnockbackState : IUnitState
{
    private Units u;
    private Vector2 force;
    private float duration;
    public KnockbackState(Units unit, Vector2 f, float d) { u = unit; force = f; duration = d; }
    public void Enter() { if (u.RB) u.RB.AddForce(force, ForceMode2D.Impulse); }
    public void Execute() { duration -= Time.deltaTime; if (duration <= 0) u.ChangeState(new MoveState(u)); }
    public void Exit() { if (u.RB) u.RB.linearVelocity = Vector2.zero; }
}