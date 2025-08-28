using UnityEngine;

[CreateAssetMenu(fileName = "CardContent", menuName ="Scriptable Object/CardContent")]
public class CardContentSO : ScriptableObject
{
    public CardContent[] cardContents;
}

[System.Serializable]
public class CardContent
{
    public string name;
    public int cost;
    public GameObject unit;
}