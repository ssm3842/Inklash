using UnityEngine;

[CreateAssetMenu(fileName = "CardContent", menuName ="Scriptable Object/CardContent")]
public class CardContentSO : ScriptableObject
{
    public CardContent[] cardContents;
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