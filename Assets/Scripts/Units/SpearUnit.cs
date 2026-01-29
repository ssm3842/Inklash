using UnityEngine;

public class SpearUnit : Units
{
    protected override void SetDefaultState() => ChangeState(new SpearMoveState(this));
}

public class SpearMoveState : MoveState
{
    private float timer = 0;
    private bool isAcc = false;
    public SpearMoveState(SpearUnit unit) : base(unit) { }

    public override void Execute()
    {
        base.Execute(); 
        
        if (u.Target == null && !u.isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= 1f && !isAcc) 
            { 
                u.StatControl.ControlBonusStat(StatType.SPD, 1);
                u.StatControl.ControlBonusStat(StatType.ATK, 1);
                isAcc = true; 
            }
        }
    }

    public override void Exit()
    {
        if (isAcc) 
        { 
            u.StatControl.ControlBonusStat(StatType.SPD, -1);
            u.StatControl.ControlBonusStat(StatType.ATK, -1);
        }
        base.Exit();
    }
}