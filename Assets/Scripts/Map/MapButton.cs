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
            { RoomType.BATTLE, icons[0] },
            { RoomType.RANDOM_EVENT, icons[4] },
            { RoomType.TREASURE, icons[1] },
            { RoomType.CAMPFIRE, icons[2] },
            { RoomType.SHOP, icons[3] },
            { RoomType.BOSS, icons[0] }
        };
    }

    public void SetRoom(RoomContent roomData)
    {
        room = roomData;
        transform.localPosition = room.position + new Vector2(100, 0);
        GetComponent<Image>().sprite = roomIcons[room.roomType];
    }

    public void OnButtonClicked()
    {
        if (!room.isInteractable) return;
        switch (room.roomType)
        {
            case RoomType.BATTLE:
                RunManager.Inst.SetupBattle();
                break;
            case RoomType.RANDOM_EVENT:
                Debug.Log("RANDOM_EVENT Clicked");
                break;
            case RoomType.TREASURE:
                Debug.Log("TREASURE Clicked");
                break;
            case RoomType.CAMPFIRE:
                Debug.Log("CAMPFIRE Clicked");
                break;
            case RoomType.SHOP:
                Debug.Log("SHOP Clicked");
                break;
            case RoomType.BOSS:
                Debug.Log("BOSS Clicked");
                break;
            default:
                Debug.Log("Bug Occured");
                break;
        }

        MapManager.Inst.LockSameFloor();
        MapManager.Inst.lastRoom = room;

        MapManager.Inst.floorClimbed++;
        MapManager.Inst.UnlockFloor(MapManager.Inst.floorClimbed);
    }
}

public class RoomContent
{
    public RoomType roomType;
    public int row;
    public int column;
    public Vector2 position;
    public List<RoomContent> nextRooms = new List<RoomContent>();
    public bool isInteractable = false;
    public bool isCleared = false;
}

public enum RoomType
{
    NOT_ASSIGNED, BATTLE, RANDOM_EVENT, TREASURE, CAMPFIRE, SHOP, BOSS,
}
