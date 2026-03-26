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

    public void InitMaxHP()
    {
        curHP = unitStats.baseMaxHp * (1 + unitStats.bonusMaxHp);
    }

    public void ControlBonusStat(StatType statType, float amount)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                unitStats.bonusMaxHp += amount;
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

    public void ControlBaseStat(StatType statType, float amount)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                unitStats.baseMaxHp *= amount;
                break;
            case StatType.ATK:
                unitStats.baseATK *= amount;
                break;
            case StatType.ATKSPD:
                unitStats.baseATKSpd *= amount;
                break;
            case StatType.RANGE:
                unitStats.baseRange *= amount;
                break;
            case StatType.SPD:
                unitStats.baseSpd *= amount;
                break;
            default:
                return ;
        }
    }

    public void ResetBonusStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                unitStats.bonusMaxHp = 0;
                break;
            case StatType.ATK:
                unitStats.bonusATK = 0;
                break;
            case StatType.ATKSPD:
                unitStats.bonusATKSpd = 0;
                break;
            case StatType.RANGE:
                unitStats.bonusRange = 0;
                break;
            case StatType.SPD:
                unitStats.bonusSpd = 0;
                break;
        }
    }

    public float GetStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                return unitStats.baseMaxHp * (1 + unitStats.bonusMaxHp);
            case StatType.ATK:
                return unitStats.baseATK * (1 + unitStats.bonusATK);
            case StatType.ATKTerm:
                return unitStats.baseATKTerm;
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
    public float baseATKTerm;
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
        baseATKTerm = newStats.baseATKTerm;
        baseATKSpd = newStats.baseATKSpd;
        baseRange = newStats.baseRange;
        baseSpd = newStats.baseSpd;
    }
    public UnitStats(float newMaxhp, float newATK, float newATKTerm, float newATKSpd, float newRange, float MoveSpd)
    {
        baseMaxHp = newMaxhp;
        baseATK = newATK;
        baseATKTerm = newATKTerm;
        baseATKSpd = newATKSpd;
        baseRange = newRange;
        baseSpd = MoveSpd;
    }
}

public enum StatType
{
    MAX_HP, ATK, ATKTerm, ATKSPD, RANGE, SPD
}