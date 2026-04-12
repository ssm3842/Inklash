using UnityEngine;

public class ShieldUnit : Units
{
    public override void Move()
    {
        // 1. 공격 중이거나 타겟이 있는 경우: 정지
        if (target != null || isAttacking)
        {
            RB.linearVelocityX = 0f;
            ANI.SetBool("IsMoving", false);
            return;
        }
        
        RB.linearVelocityX = isPlayers 
            ? statController.GetStat(StatType.SPD) 
            : -statController.GetStat(StatType.SPD);
                
        ANI.SetBool("IsMoving", true);

        if (ANI.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            RB.linearVelocityX = 0f;
            ANI.SetTrigger("Trans");
        }

    }
}
