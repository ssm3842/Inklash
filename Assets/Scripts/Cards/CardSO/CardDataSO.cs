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
    public OnDiscardType onDiscardType;
    public SealType seals;

    public Sprite cardImage; 

    public CardUIInfo firstInfo;
    public CardUIInfo secondInfo;

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
        onDiscardType = baseCardContent.onDiscardType;
        seals = baseCardContent.seals;

        cardImage = baseCardContent.cardImage;

        firstInfo = baseCardContent.firstInfo;
        secondInfo = baseCardContent.secondInfo;
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

public enum OnDiscardType
{
    None, Draw, Summon,
}

[System.Flags]
public enum SealType
{
    None         = 0,
    Burn         = 1 << 0, // 1
    Cold         = 1 << 1, // 2
    DoubleAttack = 1 << 2, // 4
    KnockBack    = 1 << 3, // 8
    Pierce       = 1 << 4, // 16
    Discard      = 1 << 5, // 32
    StartCost    = 1 << 6  // 64
}
public enum CardUIInfo
{
    None, ATK, HP, Time,
}