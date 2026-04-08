using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapButton mapButton;
    [SerializeField] GameObject mapLine;
    [SerializeField] MapDataGenerator mapGenerator;
    [SerializeField] GameObject scrollContent;

    [SerializeField]TextMeshProUGUI mapText;

    List<List<RoomContent>> mapData;
    public int floorClimbed;
    public RoomContent lastRoom;

    public void InitMapdata()
    {
        floorClimbed = 0;
        mapData = mapGenerator.GenerateMap();
        // InitImageDictionary();
        DrawMap();

        UnlockFloor(0);
    }


    void DrawMap()
    {
        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                if (room.nextRooms.Count > 0)
                    SpawnRoom(room);
            }
        }

        int middle = Mathf.FloorToInt(MapDataGenerator.MAP_WIDTH * 0.5f); //TODO
        SpawnRoom(mapData[MapDataGenerator.FLOORS - 1][middle]);
    }

    public void UnlockFloor(int floorIndex)
    {
        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                if (room.row != floorIndex) break; // 층 인덱스와 다르면 스킵.
                else
                {
                    if (floorIndex == 0) room.isInteractable = true; // 0층(시작)의 경우 모두 오픈.
                    else
                    {
                        if (lastRoom.nextRooms.Contains(room)) room.isInteractable = true; //최근 방에서 연결된 다음 방들을 연결.
                    }
                }
            }
        }
    }
    public void LockSameFloor()
    {
        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                if (room.row != floorClimbed) break;
                else
                {
                    room.isInteractable = false;
                }
            }
        }
    }

    void SpawnRoom(RoomContent room)
    {
        MapButton NewMapButton = Instantiate(mapButton, scrollContent.transform);

        if(room.roomType == RoomType.EVENT) NewMapButton.SetRoom(room);
        else NewMapButton.SetRoom(room);

        ConnectLines(room);
    }

    void ConnectLines(RoomContent room)
    {
        if (room.nextRooms.Count <= 0) return;

        foreach (RoomContent next in room.nextRooms)
        {
            GameObject newMapLine = Instantiate(mapLine, scrollContent.transform);

            newMapLine.transform.localPosition = (next.position + room.position) / 2f + new Vector2(100, 0);
            newMapLine.GetComponent<Image>().sprite = SpriteDataContainer.Inst.GetMapLineSprite();

            Vector2 direction = next.position - room.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newMapLine.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // newMapLine.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
        }
    }
    
    public void EnterRoom(RoomContent room)
    {
        LockSameFloor();
        lastRoom = room;

        switch (room.roomType)
        {
            case RoomType.BATTLE:
                SetMapText("전 투");
                RunManager.Inst.battleManager.InitBattle(false);
                break;
            case RoomType.EVENT:
                RunManager.Inst.eventCanvas.SetEventCanvas(room);
                break;
            case RoomType.SHOP:
                SetMapText("상 점");
                RunManager.Inst.shopCanvas.EnterShop();
                break;
            case RoomType.BOSS:
                SetMapText("전 투");
                RunManager.Inst.battleManager.InitBattle(true);
                break;
            default:
                Debug.Log("Bug Occured");
                break;
        }
        gameObject.SetActive(false);
    }

    public void ClearLastRoom()
    {
        lastRoom.isCleared = true;

        floorClimbed++;
        UnlockFloor(floorClimbed);
    }

    public void FailLastRoom()
    {
        lastRoom.isFailed = true;

        floorClimbed++;
        UnlockFloor(floorClimbed);
    }

    public void SetVisible() //캔버스의 열리는 여부 결정. 열릴 경우 시간 흐름 정지.
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;

            SetMapText("지 도");

            foreach(Transform child in scrollContent.transform)
            {
                child.GetComponent<MapButton>()?.UpdateAnimation();
            }
        }

        else Time.timeScale = 1f;
    }

    public void SetMapText(string text)
    {
        mapText.text = text;
    }
}
