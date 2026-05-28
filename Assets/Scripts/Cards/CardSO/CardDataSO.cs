using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName ="Scriptable Object/CardData")]
public class CardDataSO : ScriptableObject
{
    public CardContent card;
}

[System.Serializable]
public class CardContent
{
    public string id;
    public string name;
    public int cost;
    public CardType cardType;
    public AttackType attackType;
    public string effectID;
    public string description;
    public UnitStats stats;
    public GameObject unit;
    public List<SealType> seals;
    public bool isCopied;

    public Sprite cardImage; 

    public CardUIInfo firstInfo;
    public CardUIInfo secondInfo;

    [NonSerialized] bool hasOriginalValues;
    [NonSerialized] int originalCost;
    [NonSerialized] float originalBaseATK;
    [NonSerialized] float originalBaseMaxHp;

    public CardContent(CardContent baseCardContent)
    {
        id = baseCardContent.id;
        name = baseCardContent.name;
        cost = baseCardContent.cost;
        cardType = baseCardContent.cardType;
        attackType = baseCardContent.attackType;
        effectID = baseCardContent.effectID;
        description = baseCardContent.description;
        stats = new UnitStats(baseCardContent.stats);
        unit = baseCardContent.unit;
        isCopied = false;
        seals = new List<SealType>(baseCardContent.seals);

        cardImage = baseCardContent.cardImage;

        firstInfo = baseCardContent.firstInfo;
        secondInfo = baseCardContent.secondInfo;

        if(baseCardContent.hasOriginalValues)
        {
            hasOriginalValues = true;
            originalCost = baseCardContent.originalCost;
            originalBaseATK = baseCardContent.originalBaseATK;
            originalBaseMaxHp = baseCardContent.originalBaseMaxHp;
        }
        else
        {
            SetOriginalValues(baseCardContent.cost, baseCardContent.stats.baseATK, baseCardContent.stats.baseMaxHp);
        }
    }

    public bool IsCostUpgraded()
    {
        EnsureOriginalValues();
        return cost < originalCost;
    }

    public bool IsATKUpgraded()
    {
        EnsureOriginalValues();
        return stats.baseATK > originalBaseATK && !Mathf.Approximately(stats.baseATK, originalBaseATK);
    }

    public bool IsHPUpgraded()
    {
        EnsureOriginalValues();
        return stats.baseMaxHp > originalBaseMaxHp && !Mathf.Approximately(stats.baseMaxHp, originalBaseMaxHp);
    }

    public void CaptureCurrentValuesAsOriginal()
    {
        SetOriginalValues(cost, stats.baseATK, stats.baseMaxHp);
    }

    void EnsureOriginalValues()
    {
        if(hasOriginalValues) return;

        SetOriginalValues(cost, stats.baseATK, stats.baseMaxHp);
    }

    void SetOriginalValues(int newOriginalCost, float newOriginalBaseATK, float newOriginalBaseMaxHp)
    {
        originalCost = newOriginalCost;
        originalBaseATK = newOriginalBaseATK;
        originalBaseMaxHp = newOriginalBaseMaxHp;
        hasOriginalValues = true;
    }
}

public enum CardType
{
    Unit, Spell, Word,
}

public enum AttackType
{
    None, Melee, Ranged,
}

public enum SealType
{
    None,
    Ignite,
    Weaken,
    Mark,
    Chill,
    Ultimate,
    Split,
    Explosion,
    Purity,
    Copy,
}
public enum CardUIInfo
{
    None, ATK, HP, Time,
}
