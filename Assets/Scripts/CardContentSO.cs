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
    public string type;
    public string size;
    public string effectID;
    public string description;
    public CardStats stats;
    public GameObject unit;
}

[System.Serializable]
public class CardStats 
{
    public int cost;
    public float hp;
    public float atk;
    public float atkSpd;
    public float range;
    public float spd;
}