using UnityEngine;

public class SpearUnit : Units
{

    [SerializeField] float speedStep = 0.2f;     
    [SerializeField] float attackStep = 0.5f;     

    [SerializeField] float maxSpeedLimit = 2.0f; 

    private float accSpeed = 0f;
    private float accAttack = 0f;

    
    private float updateTimer = 0f;


    public override void Move()
    {
        if (target || isAttacking)
        {
            base.Move();
            return;
        }

        updateTimer += Time.deltaTime;
        if (updateTimer >= 0.1f)
        {
            updateTimer = 0f;

            if (statController.GetStat(StatType.SPD) < maxSpeedLimit)
            {
                statController.ControlBonusStat(StatType.SPD, speedStep);
                statController.ControlBonusStat(StatType.ATK, attackStep);

                accSpeed += speedStep;
                accAttack += attackStep;
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
