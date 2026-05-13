using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPattern", menuName = "Scriptable Object/Enemy Pattern")]
public class EnemyPatternSO : ScriptableObject
{
    public List<SpawnEntry> entries;
    
    [Tooltip("이 패턴이 선택될 가중치 (기본 1)")]
    public float weight = 1f;

    [Tooltip("패턴 종료 후 다음 패턴까지 대기 시간 (초)")]
    public float interPatternDelay = 0f;
}

[System.Serializable]
public class SpawnEntry
{
    public CardDataSO enemyCard;

    [Tooltip("이 항목에서 동시에 스폰할 마릿수")]
    public int count = 1;

    [Tooltip("이 항목 후 다음 항목까지 대기 시간 (초)")]
    public float delayAfter = 3.0f;

    [Tooltip("선택: 이 적에게 부착할 인장")]
    public List<SealType> extraSeals;
}