using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] MapButton mapButton;
    [SerializeField] GameObject mapLine;
    [SerializeField] MapDataGenerator mapGenerator;
    [SerializeField] GameObject scrollContent;

    List<List<RoomContent>> mapData;
    public int floorClimbed;
    public RoomContent lastRoom;

    void Start()
    {

        floorClimbed = 0;
        mapData = mapGenerator.GenerateMap();
        VisualizeMap();

        UnlockFloor(0);

        gameObject.SetActive(false);
    }

    void VisualizeMap()
    {
        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                if (room.nextRooms.Count > 0)
                    SpawnRoom(room);
            }
        }

        int middle = Mathf.FloorToInt(7 * 0.5f); //TODO
        SpawnRoom(mapData[15 - 1][middle]);
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
        NewMapButton.SetRoom(room);

        ConnectLines(room);
    }

    void ConnectLines(RoomContent room)
    {
        if (room.nextRooms.Count <= 0) return;

        foreach (RoomContent next in room.nextRooms)
        {
            GameObject newMapLine = Instantiate(mapLine, scrollContent.transform);

            newMapLine.transform.localPosition = (next.position + room.position) / 2f + new Vector2(150, 50);

            Vector2 direction = next.position - room.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newMapLine.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
    

    public void SetVisible() //캔버스의 열리는 여부 결정. 열릴 경우 시간 흐름 정지.
    {
        if (!gameObject.activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        gameObject.SetActive(!gameObject.activeSelf);
    }
}
