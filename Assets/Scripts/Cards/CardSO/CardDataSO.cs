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
    Cold,
    ExtraHit,
    KnockBack,
    Pierce,
    Weak,
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