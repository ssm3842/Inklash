using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] Animator ani;
    [SerializeField] Image image;

    [SerializeField] GameObject clearMark;
    [SerializeField] GameObject failMark;
    RoomContent room;

    public void UpdateAnimation()
    {
        if(room.isInteractable) ani.Play("MapButtonOpen");
        else ani.Play("MapButtonDefault");

        if(room.isCleared) clearMark.SetActive(true);
        else if(room.isFailed) failMark.SetActive(true);
    }

    public void SetRoom(RoomContent roomData, Sprite buttonImage)
    {
        room = roomData;

        transform.localPosition = room.position + new Vector2(100, 0);

        if(room.roomType == RoomType.BOSS)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
            transform.localPosition += new Vector3(100, 0, 0);
        }

        image.sprite = buttonImage;
    }

    public void OnButtonClicked()
    {
        if (!room.isInteractable) return;
        RunManager.Inst.mapManager.EnterRoom(room);
    }
}

public class RoomContent
{
    public RoomType roomType;
    public EventRoomType eventRoomType;
    public int row;
    public int column;
    public Vector2 position;
    public List<RoomContent> nextRooms = new List<RoomContent>();
    public bool isInteractable = false;
    public bool isCleared = false;
    public bool isFailed = false;
}

public enum RoomType
{
    NOT_ASSIGNED, BATTLE, SHOP, EVENT, BOSS,
}
public enum EventRoomType
{
    NOT_EVENT, CAMPFIRE, ADDCARD, MIXCARD, MAKESEAL,
}
