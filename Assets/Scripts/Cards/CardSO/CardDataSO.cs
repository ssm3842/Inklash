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

[System.Flags]
public enum SealType
{
    None         = 0,
    Ignite       = 1 << 0, // 1
    Cold         = 1 << 1, // 2
    ExtraHit     = 1 << 2, // 4
    KnockBack    = 1 << 3, // 8
    Pierce       = 1 << 4, // 16
    Weak         = 1 << 7, // 128
    Mark         = 1 << 8, // 256
    Chill        = 1 << 9, // 512
    Ultimate      = 1 << 10, // 1024
    Split        = 1 << 11, // 2048
    Explosion     = 1 << 12, // 4096
    Purity      = 1 << 13, // 8192
    Copy        = 1 << 14, // 16384
}
public enum CardUIInfo
{
    None, ATK, HP, Time,
}