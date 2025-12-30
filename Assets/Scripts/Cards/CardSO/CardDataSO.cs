using System;
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
    public CardType type;
    public string effectID;
    public string description;
    public UnitStats stats;
    public GameObject unit;
}

public enum CardType
{
    Unit, Spell, Word,
}