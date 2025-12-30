using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName ="Scriptable Object/Rooms")]
public class RoomDataSO : ScriptableObject
{
    public RoomData[] roomDatas;
}

[System.Serializable]
public class RoomData
{
    public Sprite baseSprite;
    public GameObject[] availableEnemyUnits;
    public int baseHP;
}

