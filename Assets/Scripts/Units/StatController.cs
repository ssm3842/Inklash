using UnityEngine;

public class StatController : MonoBehaviour
{
    [SerializeField]UnitStats unitStats;
    [SerializeField]protected float curHP;

    public void InitStat(UnitStats stats)
    {
        unitStats = new UnitStats(stats);
        curHP = unitStats.baseMaxHp;

        unitStats.bonusMaxHp = 0;
        unitStats.bonusATK = 0;
        unitStats.bonusATKSpd = 0;
        unitStats.bonusRange = 0;
        unitStats.bonusSpd = 0;
    }

    public void ControlBonusStat(StatType statType, float amount)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                // unitStats.baseMaxHp;
                break;
            case StatType.ATK:
                unitStats.bonusATK += amount;
                break;
            case StatType.ATKSPD:
                unitStats.bonusATKSpd += amount;
                break;
            case StatType.RANGE:
                unitStats.bonusRange += amount;
                break;
            case StatType.SPD:
                unitStats.bonusSpd += amount;
                break;
            default:
                return ;
        }
    }

    public float GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                return unitStats.baseMaxHp;
            case StatType.ATK:
                return unitStats.baseATK * (1 + unitStats.bonusATK);
            case StatType.ATKSPD:
                return unitStats.baseATKSpd * (1 + unitStats.bonusATKSpd) ;
            case StatType.RANGE:
                return unitStats.baseRange * (1 + unitStats.bonusRange);
            case StatType.SPD:
                return unitStats.baseSpd * (1 + unitStats.bonusSpd);
            default:
                return 0f;
        }
    }
    public float GetCurHp(){ return curHP; }

    public void ChangeCurHp(float amount) { curHP -= amount; }
}

[System.Serializable]
public class UnitStats
{
    public float baseMaxHp;
    public float baseATK;
    public float baseATKSpd;
    public float baseRange;
    public float baseSpd;

    public float bonusMaxHp;
    public float bonusATK;
    public float bonusATKSpd;
    public float bonusRange;
    public float bonusSpd;


    public UnitStats(UnitStats newStats)
    {
        baseMaxHp = newStats.baseMaxHp;
        baseATK = newStats.baseATK;
        baseATKSpd = newStats.baseATKSpd;
        baseRange = newStats.baseRange;
        baseSpd = newStats.baseSpd;
    }
    public UnitStats(float newMaxhp, float newATK, float newATKSpd, float newRange, float MoveSpd)
    {
        baseMaxHp = newMaxhp;
        baseATK = newATK;
        baseATKSpd = newATKSpd;
        baseRange = newRange;
        baseRange = MoveSpd;
    }
}

public enum StatType
{
    MAX_HP, ATK, ATKSPD, RANGE, SPD
}