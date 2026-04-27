using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapButton : MonoBehaviour
{
    [SerializeField] Animator ani;
    [SerializeField] Image image;

    public GameObject clearMark;
    RoomContent room;

    public void UpdateAnimation()
    {
        if(room.isInteractable)
        {
            if(room.roomType == RoomType.BOSS) ani.Play("BossMapButtonOpen");
            else ani.Play("MapButtonOpen");
        }
        else ani.Play("MapButtonDefault");

        if(room.isCleared) clearMark.SetActive(true);
    }

    public void SetRoom(RoomContent roomData)
    {
        room = roomData;

        transform.localPosition = room.position + new Vector2(100, 0);

        if(room.roomType == RoomType.BOSS)
        {
            transform.localScale = new Vector3(2f, 2f, 1f);
            transform.localPosition += new Vector3(100, 0, 0);
        }

        if(room.roomType == RoomType.EVENT) { image.sprite = SpriteDataContainer.Inst.GetMapEventIconBlack(room.eventRoomType); }
        else { image.sprite = SpriteDataContainer.Inst.GetMapIconBlack(room.roomType); }
    }

    public void OnButtonClicked()
    {
        StartCoroutine(OnButtonClickedRoutine());
        RunManager.Inst.mapManager.MapbuttonClicked();
    }
    IEnumerator OnButtonClickedRoutine()
    {
        if (!room.isInteractable) yield break;

        clearMark.SetActive(true);
        clearMark.GetComponent<Animator>().SetTrigger("Clicked");

        yield return new WaitForSecondsRealtime(1.0f);
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
}

public enum RoomType
{
    NOT_ASSIGNED, BATTLE, SHOP, EVENT, BOSS,
}
public enum EventRoomType
{
    NOT_EVENT, UPGRADE, ADDCARD, MIXCARD, MAKESEAL,
}
