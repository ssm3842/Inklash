using System;
using System.Collections.Generic;
using UnityEngine;

public class MapDataGenerator : MonoBehaviour
{
    const int X_DIST = 200;
    const int Y_DIST = 150;
    const int PLACEMENT_RANDOMNESS = 25;
    public const int FLOORS = 8;
    public const int MAP_WIDTH = 4;
    const int PATHS = 4;

    const int MIN_EVENT_COUNT = 4;
    const int MAX_EVENT_COUNT = 5;
    const float EVENT_ROOMS_BASE_WEIGHT = 3f;
    int spawnedEventRoomCount = 0;

    Dictionary<RoomType, float> randomRoomTypeWeights = new Dictionary<RoomType, float>
    {
        { RoomType.BATTLE, 0f },
        { RoomType.EVENT, 0f },
    };

    Dictionary<EventRoomType, float> randomEventRoomTypeWeights = new Dictionary<EventRoomType, float>
    {
        { EventRoomType.ADDCARD, EVENT_ROOMS_BASE_WEIGHT },
        { EventRoomType.UPGRADE, EVENT_ROOMS_BASE_WEIGHT },
        { EventRoomType.MIXCARD, EVENT_ROOMS_BASE_WEIGHT },
        { EventRoomType.MAKESEAL, EVENT_ROOMS_BASE_WEIGHT },
    };

    [SerializeField] List<List<RoomContent>> mapData;

    public List<List<RoomContent>> GenerateMap()
    {
        mapData = GenerateInitialGrid();    //맵 그리드 데이터 생성 
        List<int> startingPoints = GetRandomStartingPoints(); // 랜덤한 스타팅 포인트 생성, 최소 2개, 최대 6개 

        foreach (int j in startingPoints)
        {
            int currentJ = j;
            for (int i = 0; i < FLOORS - 1; i++)
            {
                currentJ = SetupConnection(i, currentJ); //맵 노드끼리 연결 생성.
            }
        }

        SetupBossRoom(); //최상단에 보스룸 생성

        SetupRoomTypes(); //맵 타입 설정.

        return mapData;
    }

    List<List<RoomContent>> GenerateInitialGrid()
    {
        List<List<RoomContent>> result = new List<List<RoomContent>>();

        for (int i = 0; i < FLOORS; i++)
        {
            List<RoomContent> adjacentRooms = new List<RoomContent>();

            for (int j = 0; j < MAP_WIDTH; j++)
            {
                RoomContent roomContent = new RoomContent();
                Vector2 offset = new Vector2(600f, 320f) + new Vector2(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)) * PLACEMENT_RANDOMNESS;

                roomContent.position = new Vector2(i * X_DIST, j * Y_DIST) + offset ;
                roomContent.row = i;
                roomContent.column = j;

                if (i == FLOORS - 1) roomContent.position.y = (j + 1) * Y_DIST + 130f; //보스룸의 경우 위치 고정.

                adjacentRooms.Add(roomContent);
            }
            result.Add(adjacentRooms);
        }
        return result;
    }

    List<int> GetRandomStartingPoints()
    {
        List<int> yCoordinates = new List<int>();
        int uniquePoints = 0;

        while (uniquePoints < 2)
        {
            uniquePoints = 0;
            yCoordinates = new List<int>();

            for (int i = 0; i < PATHS; i++)
            {
                int startingPoint = UnityEngine.Random.Range(0, MAP_WIDTH - 1);
                if (!yCoordinates.Contains(startingPoint)) uniquePoints += 1;
                yCoordinates.Add(startingPoint);
            }
        }
        return yCoordinates;
    }

    int SetupConnection(int row, int column)
    {
        RoomContent nextRoom = null;
        RoomContent currentRoom = mapData[row][column];

        while (nextRoom == null || WouldCrossExistingPath(row, column, nextRoom))
        {
            int randomJ = Mathf.Clamp(UnityEngine.Random.Range(column - 1, column + 2), 0, MAP_WIDTH - 1); //양쪽 끝에서는 확률 이상할 수 있음. 이후 수정.
            nextRoom = mapData[row + 1][randomJ];
        }
        currentRoom.nextRooms.Add(nextRoom);

        return nextRoom.column;
    }

    bool WouldCrossExistingPath(int row, int column, RoomContent nextRoom)
    {
        RoomContent leftNeighbor = null;
        RoomContent rightNeighbor = null;

        if (column > 0) leftNeighbor = mapData[row][column - 1];
        if (column < MAP_WIDTH - 1) rightNeighbor = mapData[row][column + 1];

        if (rightNeighbor != null && nextRoom.column > column) //오른쪽에 노드가 있고 다음 방이 그 앞에 있다면 검사 실행.
        {
            foreach (RoomContent rightsNextRoom in rightNeighbor.nextRooms) //오른쪽 노드의 다음 방이 자신의 다음 방과 교차되는지 검사.
            {
                if (rightsNextRoom.column < nextRoom.column)
                    return true;
            }
        }

        if (leftNeighbor != null && nextRoom.column < column)
        {
            foreach (RoomContent leftsNextRoom in leftNeighbor.nextRooms) //왼쪽 노드의 다음 방이 자신의 다음 방과 교차되는지 검사.
            {
                if (leftsNextRoom.column > nextRoom.column)
                    return true;
            }
        }
        return false;
    }

    void SetupBossRoom()
    {
        int middle = Mathf.FloorToInt(MAP_WIDTH * 0.5f);
        RoomContent bossRoom = mapData[FLOORS - 1][middle];

        for (int column = 0; column < MAP_WIDTH; column++)
        {
            RoomContent currentRoom = mapData[FLOORS - 2][column];
            if (currentRoom.nextRooms.Count > 0)
            {
                currentRoom.nextRooms = new List<RoomContent> { bossRoom };
            }
        }
        bossRoom.roomType = RoomType.BOSS;
    }

    void SetupRoomTypes()
    {
        //시작은 항상 전투.
        foreach (RoomContent room in mapData[0]) 
        {
            if (room.nextRooms.Count > 0) room.roomType = RoomType.BATTLE;
        }

        //5번째 노드는 항상 상점.
        foreach (RoomContent room in mapData[4]) 
        {
            if (room.nextRooms.Count > 0) room.roomType = RoomType.BOSS;
        }

        //이벤트 방 개수 보정을 위한 리스트
        List<RoomContent> battleRoomList = new List<RoomContent>();
        foreach(RoomContent room in battleRoomList)
        {
            if(room.nextRooms.Count <= 0) battleRoomList.Remove(room);
        }

        //할당되지 않은 방의 타입을 결정.
        foreach (List<RoomContent> currentFloor in mapData) 
        {
            foreach (RoomContent room in currentFloor)
            {
                foreach (RoomContent nextRoom in room.nextRooms)
                {
                    if (nextRoom.roomType == RoomType.NOT_ASSIGNED) nextRoom.roomType = RoomType.BATTLE;
                    // SetRoomRandomly(nextRoom);

                    //전투가 할당된 방만 리스트에 저장.
                    if (nextRoom.roomType == RoomType.BATTLE) battleRoomList.Add(nextRoom);
                }
            }
        }

        //이벤트 방의 갯수를 4-6개로 생성.
        while(spawnedEventRoomCount < MAX_EVENT_COUNT)
        {
            //최소 생성 개수보다 적은 경우 확정 변경.
            if(spawnedEventRoomCount < MIN_EVENT_COUNT)
            {
                int randomIndex = UnityEngine.Random.Range(0, battleRoomList.Count);
                RoomContent targetRoom = battleRoomList[randomIndex];

                SetEventRoom(targetRoom);
                battleRoomList.Remove(targetRoom);
            }
            //4개 이상인 경우 확률적으로 추가. 6개를 넘으면 종료.
            else
            {
                //30퍼센트 확률로 이벤트 노드 생성.
                if(UnityEngine.Random.Range(0, 100) < 30)
                {
                    int randomIndex = UnityEngine.Random.Range(0, battleRoomList.Count);
                    RoomContent targetRoom = battleRoomList[randomIndex];

                    SetEventRoom(targetRoom);
                    battleRoomList.Remove(targetRoom);
                }
                else spawnedEventRoomCount ++;
            }
        }
    }

    // void SetRoomRandomly(RoomContent roomToSet)
    // {
    //     // bool cantSpawnCampfireBelow4 = true; //4층이하에서 휴식 생성 불가 
    //     // bool cantConsecutiveCampfire = true; //연속 휴식 생성 불가
    //     // bool cantConsecutiveShop = true; //연속 상점 생성 불가
    //     // bool cantCampfireOn12 = true; //13층에 휴식 고정이므로 12층에 휴식 생성 불가.

    //     RoomType typeCandidate = RoomType.NOT_ASSIGNED;

    //     typeCandidate = GetRandomRoomTypeByWeight();

    //     // while (cantSpawnCampfireBelow4 || cantConsecutiveCampfire || cantConsecutiveShop || cantCampfireOn12) //규칙을 어기지 않을 때까지 반복.
    //     // {
    //     //     typeCandidate = GetRandomRoomTypeByWeight();

    //     //     bool isCampfire = (typeCandidate == RoomType.NOT_ASSIGNED);
    //     //     bool hasCampFireParent = RoomHasParentOfType(roomToSet, RoomType.NOT_ASSIGNED);
    //     //     bool isShop = typeCandidate == RoomType.SHOP;
    //     //     bool hasShopParent = RoomHasParentOfType(roomToSet, RoomType.SHOP);

    //     //     cantSpawnCampfireBelow4 = isCampfire && roomToSet.row < 3;
    //     //     cantConsecutiveCampfire = isCampfire && hasCampFireParent;
    //     //     cantConsecutiveShop = isShop && hasShopParent;
    //     //     cantCampfireOn12 = isCampfire && roomToSet.row == 12;
    //     // }

    //     roomToSet.roomType = typeCandidate;

    //     //이벤트 노드일 경우 하나를 랜덤으로 선택.
    //     if(typeCandidate == RoomType.EVENT) SetEventRoom(roomToSet);
    //     else roomToSet.eventRoomType = EventRoomType.NOT_EVENT;
    // }

    void SetEventRoom(RoomContent roomToSet)
    {
        roomToSet.roomType = RoomType.EVENT;

        spawnedEventRoomCount ++;

        EventRoomType targetEventRoomType = GetRandomEventRoomTypeByWeight();
        roomToSet.eventRoomType = targetEventRoomType;
    }

    bool RoomHasParentOfType(RoomContent room, RoomType type) //아래층 부모 노드에 특정 방 타입이 있는지 검사
    {
        List<RoomContent> parents = new List<RoomContent>();

        if (room.column > 0 && room.row > 0)
        {
            RoomContent parentCandidate = mapData[room.row - 1][room.column - 1];
            if (parentCandidate.nextRooms.Contains(room))
                parents.Add(parentCandidate);
        }
        if (room.row > 0)
        {
            RoomContent parentCandidate = mapData[room.row - 1][room.column];
            if (parentCandidate.nextRooms.Contains(room))
                parents.Add(parentCandidate);
        }
        if (room.column < MAP_WIDTH - 1 && room.row > 0)
        {
            RoomContent parentCandidate = mapData[room.row - 1][room.column + 1];
            if (parentCandidate.nextRooms.Contains(room))
                parents.Add(parentCandidate);
        }

        foreach (RoomContent parent in parents)
        {
            if (parent.roomType == type) return true;
        }

        return false;
    }

    EventRoomType GetRandomEventRoomTypeByWeight()
    {
        float total = 0f;
        foreach(float value in randomEventRoomTypeWeights.Values) total += value;

        float roll = UnityEngine.Random.Range(0f, total);
        float current = 0f;

        foreach (EventRoomType type in randomEventRoomTypeWeights.Keys)
        {
            current += randomEventRoomTypeWeights[type];
            if (roll <= current)
            {
                //선택된 이벤트 타입의 확률을 감소시킴.
                if (randomEventRoomTypeWeights[type] >= 3f) randomEventRoomTypeWeights[type] = 0.1f;

                return type;
            }
        }

        return EventRoomType.ADDCARD;
    }
}
