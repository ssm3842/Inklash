using System.Collections.Generic;
using UnityEngine;

public class MapVisualGenerator : MonoBehaviour
{
    [SerializeField] MapButton mapButton;
    [SerializeField] GameObject mapLine;
    [SerializeField] MapDataGenerator mapGenerator;

    List<List<RoomContent>> mapData;

    void Start()
    {
        mapData = mapGenerator.GenerateMap();
        VisualizeMap();

        UnlockFloor(0);
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

    void UnlockFloor(int floorIndex)
    {
        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                if (room.row != floorIndex) break;
                else
                {
                    // room.selected = 
                }
            }
        }
    }

    void UnlockNextRooms()
    {
        return;
    }

    void SpawnRoom(RoomContent room)
    {
        MapButton NewMapButton = Instantiate(mapButton, this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0));
        NewMapButton.SetRoom(room);

        ConnectLines(room);
    }

    void ConnectLines(RoomContent room)
    {
        if (room.nextRooms.Count <= 0) return;

        foreach (RoomContent next in room.nextRooms)
        {
            GameObject newMapLine = Instantiate(mapLine, this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0));

            newMapLine.transform.localPosition = (next.position + room.position) / 2f  + new Vector2(150, 50);

            Vector2 direction = next.position - room.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newMapLine.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
