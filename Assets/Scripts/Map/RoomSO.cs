using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rooms", menuName ="Scriptable Object/Rooms")]
public class RoomsSO : ScriptableObject
{
    public RoomContent[] cardContents;
}

[System.Serializable]
public class RoomContent
{
    public RoomType roomType;
    public int row;
    public int column;
    public Vector2 position;
    public List<RoomContent> nextRooms;
    public bool selected = false;
}

public enum RoomType
{
    NOT_ASSIGNED, MONSTER, TREASURE, CAMPFIRE, SHOP, BOSS,
}

