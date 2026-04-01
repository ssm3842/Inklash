using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName ="Scriptable Object/EnemyBaseData")]
public class EnemyBaseDataSO : ScriptableObject
{
    public Sprite baseSprite;

    public float maxHP;
    public bool isBoss = false; 

    public List<EnemyPatternSO> startPatterns;
    public List<EnemyPatternSO> normalPatterns;
    public List<EnemyPatternSO> phase2Patterns;
}