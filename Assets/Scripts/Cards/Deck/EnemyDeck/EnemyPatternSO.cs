using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPattern", menuName = "Scriptable Object/Enemy Pattern")]
public class EnemyPatternSO : ScriptableObject
{
    public List<CardDataSO> enemyDeck;
}