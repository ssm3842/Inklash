using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName ="Scriptable Object/Deck")]
public class DeckSO : ScriptableObject
{
    public List<string> cardId;
}

