using UnityEngine;

public class SpearUnit : Units
{
    private float accSpeed = 0f;
    private float accAttack = 0f;

    float stepSpd = 0.1f;
    float stepAtk = 0.1f;

    private float updateTimer = 0f;


    public override void Move()
    {
        if (target || isAttacking)
        {
            base.Move();
            return;
        }
        
        float currentSpd = statController.GetStat(StatType.SPD);

        if (currentSpd <= 0f)
        {
            if (accSpeed > 0f || updateTimer > 0f)
            {
                ResetBonus();
            }
        }

        updateTimer += Time.deltaTime;
       if (updateTimer >= 0.1f)
        {
            updateTimer = 0f;

            if (accSpeed < 0.5)
            {
                statController.ControlBonusStat(StatType.SPD, stepSpd);
                statController.ControlBonusStat(StatType.ATK, stepAtk);

                accSpeed += stepSpd;
                accAttack += stepAtk;
            }
        }

        base.Move();
    }

    public override void _AttackEnemy()
    {
        base._AttackEnemy(); 
        ResetBonus(); 
    }

    private void ResetBonus()
    {
        statController.ControlBonusStat(StatType.SPD, -accSpeed);
        statController.ControlBonusStat(StatType.ATK, -accAttack);
            
        accSpeed = 0f;
        accAttack = 0f;

        updateTimer = 0f;
    }
}
