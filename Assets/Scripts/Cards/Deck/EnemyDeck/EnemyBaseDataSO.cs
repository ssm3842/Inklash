using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName ="Scriptable Object/EnemyBaseData")]
public class EnemyBaseDataSO : ScriptableObject
{
    public Sprite baseSprite;
    public List<CardDataSO> enemyDeck;
    public int startingHP;
}