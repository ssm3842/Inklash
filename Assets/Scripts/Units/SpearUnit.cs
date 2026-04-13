using UnityEngine;

public class SpearUnit : Units
{
    private bool isAccelerated = false;
    private float updateTimer = 0f;

    public override void OnDisruptEffect()
    {
        ResetBonus();
        base.OnDisruptEffect();
    }

    public override void Move()
    {
        if (target || isAttacking)
        {
            updateTimer = 0f;
            RB.linearVelocityX = 0f;
            ANI.SetBool("IsMoving", false);
            ANI.SetBool("IsDashing", false);
            return;
        }    

        RB.linearVelocityX = isPlayers 
        ? statController.GetStat(StatType.SPD) 
        : -statController.GetStat(StatType.SPD);

        ANI.SetBool("IsMoving", true);

       updateTimer += Time.deltaTime;
        if (updateTimer >= 1f && !isAccelerated)
        {
            StartAcceleration();
        }

        if (!IsDead) ANI.SetBool("IsDashing", isAccelerated);
    }

    private void StartAcceleration()
    {
        statController.ControlBonusStat(StatType.SPD, 1);
        statController.ControlBonusStat(StatType.ATK, 1);
        isAccelerated = true;

        ANI.SetTrigger("StartTrans");
    }

    protected override void PlayAttackAnimation()
    {
        if (isAccelerated)
        {
            ANI.SetTrigger("Attack2");
        }
        else
        {
            ANI.SetTrigger("Attacked");
        }
    }

    public override void _AttackEnemy()
    {
        base._AttackEnemy(); 
        ResetBonus(); 
    }

    private void ResetBonus()
    {
        if(isAccelerated){
        statController.ControlBonusStat(StatType.SPD, -1);
        statController.ControlBonusStat(StatType.ATK, -1);

        isAccelerated = false;
        updateTimer = 0f;

        ANI.SetBool("IsDashing", false);
        }
    }
}
