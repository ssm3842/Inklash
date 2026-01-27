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
            base.Move();
            return;
        }    

        updateTimer += Time.deltaTime;
       if (updateTimer >= 1f)
        {
            updateTimer = 0f;

            if (!isAccelerated)
            {
                statController.ControlBonusStat(StatType.SPD, 1);
                statController.ControlBonusStat(StatType.ATK, 1);
                isAccelerated = true;
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
        if(isAccelerated){
        statController.ControlBonusStat(StatType.SPD, -1);
        statController.ControlBonusStat(StatType.ATK, -1);

        isAccelerated = false;
        updateTimer = 0f;
        }
    }
}
