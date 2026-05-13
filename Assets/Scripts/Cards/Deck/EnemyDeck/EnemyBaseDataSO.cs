using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName ="Scriptable Object/EnemyBaseData")]
public class EnemyBaseDataSO : ScriptableObject
{
    public Sprite baseSprite;
    public float maxHP;
    public bool isBoss = false;
    
    [Header("층별 배율")]                   
    public float hpMultiplier = 1.0f;            
    public float atkMultiplier = 1.0f;            
    
    [Header("어느 층에서 등장하는가")]              
    public int minFloor = 1;                       
    public int maxFloor = 8;                      

    public List<EnemyPatternSO> startPatterns;
    public List<EnemyPatternSO> normalPatterns;
    public List<EnemyPatternSO> phase2Patterns;
}