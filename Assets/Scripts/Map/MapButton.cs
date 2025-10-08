using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] Sprite[] icons = new Sprite[5];
    Dictionary<RoomType, Sprite> roomIcons;

    RoomContent room;

    void Awake()
    {
        roomIcons = new Dictionary<RoomType, Sprite>
        {
            { RoomType.NOT_ASSIGNED, null },
            { RoomType.MONSTER, icons[0] },
            { RoomType.TREASURE, icons[1] },
            { RoomType.CAMPFIRE, icons[2] },
            { RoomType.SHOP, icons[3] },
            { RoomType.BOSS, icons[0] }
        };
    }

    public void SetRoom(RoomContent roomData)
    {
        room = roomData;
        transform.position = room.position;
        GetComponent<Image>().sprite = roomIcons[room.roomType];
    }

    public void OnButtonClicked()
    {
        Debug.Log("Button Clicked");
    }
}

public class RoomContent
{
    public RoomType roomType;
    public int row;
    public int column;
    public Vector2 position;
    public List<RoomContent> nextRooms = new List<RoomContent>();
    public bool selected = false;
}

public enum RoomType
{
    NOT_ASSIGNED, MONSTER, TREASURE, CAMPFIRE, SHOP, BOSS,
}
