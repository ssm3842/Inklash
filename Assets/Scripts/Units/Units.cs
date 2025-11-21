using UnityEngine;
using System.Collections;

public class Units : Entity
{
    [SerializeField] private float ATK;
    private float MoveSPD;
    private float AtkSPD;
    private float Range;

    private bool isAttacking = false;

    [SerializeField] private bool canAttack;
    [SerializeField]private float canAttackTimer;

    private bool isDead = false;       
    public bool IsDead => isDead;
    private SpriteRenderer SR;        
    private Collider2D COL;           
    [SerializeField] private float deathDuration = 0.3f; 

    Rigidbody2D RB;
    Animator ANI;

    Entity target;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        ANI = GetComponent<Animator>();

        SR = GetComponent<SpriteRenderer>();
        COL = GetComponent<Collider2D>();
    }

    override public void Init(bool isplayers, CardContent card)
    {
        this.ATK = card.stats.atk;
        this.MoveSPD = card.stats.spd;
        this.AtkSPD = card.stats.atkSpd;
        ANI.SetFloat("AtkSpd", AtkSPD);
        this.Range = card.stats.range;

        isDead = false; 

        canAttack = true;
        canAttackTimer = 0f;
        base.Init(isplayers, card);
        
        isPlayers = isplayers;
    }

    void Update()
    {
        if (isDead) return;
        
if (target != null)
        {

            Units targetUnit = target.GetComponent<Units>();
            
            if (targetUnit != null && targetUnit.IsDead)
            {
                target = null;    
                isAttacking = false; 
            }
        }

        if (!target && !isAttacking) RB.linearVelocityX = isPlayers ? MoveSPD : -MoveSPD; //목표가 없으면 이동.
        else RB.linearVelocityX = 0f;   //목표가 있으면 정지

        ANI.SetBool("IsMoving", RB.linearVelocity.magnitude > 0f);

        // RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, isPlayers ? Vector3.right : Vector3.left, Range); //레이캐스트로 타겟 검사
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
        if (isDead) return;

        if (CurHP <= amount) Die(); //Destroy(this.gameObject);
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

//사망모션 임의 구현
    private void Die()
    {
        isDead = true; 

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
}