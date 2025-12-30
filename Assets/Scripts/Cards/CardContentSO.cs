using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardContent", menuName ="Scriptable Object/CardContent")]
public class CardDataLinkSO : ScriptableObject
{
    public CardLink[] playerUnits;
    public CardLink[] playerSpells;
    public CardLink[] playerWords;
    public CardLink[] EnemyUnits;
}

[Serializable]
public class CardLink
{
    public string id;
    public CardDataSO cardContents;
}

[Flags]
public enum WordCardType
{
    None = 0,
    Unit = 1 << 0,
    Spell = 1 << 1,
    Card = 1 << 2,
}