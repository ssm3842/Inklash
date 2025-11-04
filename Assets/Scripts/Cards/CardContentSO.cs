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
    public CardStats stats;
    public GameObject unit;
}

[System.Serializable]
public class CardStats
{
    public float hp;
    public float atk;
    public float atkSpd;
    public float range;
    public float spd;
}

public enum CardType
{
    Unit, Spell, Word,
}