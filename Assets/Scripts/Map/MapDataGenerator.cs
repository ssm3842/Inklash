using System.Collections.Generic;
using UnityEngine;

public class MapDataGenerator : MonoBehaviour
{
    const int X_DIST = 150;
    const int Y_DIST = 150;
    const int PLACEMENT_RANDOMNESS = 25;
    const int FLOORS = 15;
    const int MAP_WIDTH = 7;
    const int PATHS = 6;
    const float BATTLE_ROOM_WEIGHT = 10;
    const float RANDOM_EVENT_ROOM_WEIGHT = 7.5f;
    const float SHOP_ROOM_WEIGHT = 2.5f;
    const float CAMPFIRE_ROOM_WEIGHT = 4f;

    Dictionary<RoomType, float> randomRoomTypeWeights = new Dictionary<RoomType, float>
    {
        { RoomType.BATTLE, 0f },
        { RoomType.RANDOM_EVENT, 0f },
        { RoomType.CAMPFIRE, 0f },
        { RoomType.SHOP, 0f}
    };

    float randomRoomTypeTotalWeight = 0f;
    [SerializeField] List<List<RoomContent>> mapData;

    public List<List<RoomContent>> GenerateMap()
    {
        mapData = GenerateInitialGrid();    //15*7 사이즈의 맵 그리드 데이터 생성 
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
        SetupRandomRoomWeights(); //맵 타입 가중치 설정.
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
                Vector2 offset = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f)) * PLACEMENT_RANDOMNESS;

                roomContent.position = new Vector2(j * X_DIST, i * Y_DIST) + offset ;
                roomContent.row = i;
                roomContent.column = j;

                if (i == FLOORS - 1) roomContent.position.y = (i + 1) * Y_DIST; //보스룸의 경우 위치 고정.

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
                int startingPoint = Random.Range(0, MAP_WIDTH - 1);
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
            int randomJ = Mathf.Clamp(Random.Range(column - 1, column + 2), 0, MAP_WIDTH - 1); //양쪽 끝에서는 확률 이상할 수 있음. 이후 수정.
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
    void SetupRandomRoomWeights()
    {
        randomRoomTypeWeights[RoomType.BATTLE] = BATTLE_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomType.RANDOM_EVENT] = randomRoomTypeWeights[RoomType.BATTLE] + RANDOM_EVENT_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomType.CAMPFIRE] = randomRoomTypeWeights[RoomType.RANDOM_EVENT] + CAMPFIRE_ROOM_WEIGHT;
        randomRoomTypeWeights[RoomType.SHOP] = randomRoomTypeWeights[RoomType.CAMPFIRE] + SHOP_ROOM_WEIGHT;

        randomRoomTypeTotalWeight = randomRoomTypeWeights[RoomType.SHOP];
    }

    void SetupRoomTypes()
    {
        foreach (RoomContent room in mapData[0]) //시작은 항상 전투.
        {
            if (room.nextRooms.Count > 0) room.roomType = RoomType.BATTLE;
        }

        foreach (RoomContent room in mapData[8]) //9번째는 항상 상자방.
        {
            if (room.nextRooms.Count > 0) room.roomType = RoomType.TREASURE;
        }

        foreach (RoomContent room in mapData[13]) //13번째(보스 전)는 항상 휴식.
        {
            if (room.nextRooms.Count > 0) room.roomType = RoomType.CAMPFIRE;
        }

        foreach (List<RoomContent> currentFloor in mapData)
        {
            foreach (RoomContent room in currentFloor)
            {
                foreach (RoomContent nextRoom in room.nextRooms)
                {
                    if (nextRoom.roomType == RoomType.NOT_ASSIGNED)
                        SetRoomRandomly(nextRoom);
                }
            }
        }
    }

    void SetRoomRandomly(RoomContent roomToSet)
    {
        bool cantSpawnCampfireBelow4 = true; //4층이하에서 휴식 생성 불가 
        bool cantConsecutiveCampfire = true; //연속 휴식 생성 불가
        bool cantConsecutiveShop = true; //연속 상점 생성 불가
        bool cantCampfireOn12 = true; //13층에 휴식 고정이므로 12층에 휴식 생성 불가.

        RoomType typeCandidate = RoomType.NOT_ASSIGNED;

        while (cantSpawnCampfireBelow4 || cantConsecutiveCampfire || cantConsecutiveShop || cantCampfireOn12) //규칙을 어기지 않을 때까지 반복.
        {
            typeCandidate = GetRandomRoomTypeByWeight();

            bool isCampfire = typeCandidate == RoomType.CAMPFIRE;
            bool hasCampFireParent = RoomHasParentOfType(roomToSet, RoomType.CAMPFIRE);
            bool isShop = typeCandidate == RoomType.SHOP;
            bool hasShopParent = RoomHasParentOfType(roomToSet, RoomType.SHOP);

            cantSpawnCampfireBelow4 = isCampfire && roomToSet.row < 3;
            cantConsecutiveCampfire = isCampfire && hasCampFireParent;
            cantConsecutiveShop = isShop && hasShopParent;
            cantCampfireOn12 = isCampfire && roomToSet.row == 12;
        }
        roomToSet.roomType = typeCandidate;
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

    RoomType GetRandomRoomTypeByWeight()
    {
        float roll = Random.Range(0f, randomRoomTypeTotalWeight);

        foreach (RoomType type in randomRoomTypeWeights.Keys)
        {
            if (randomRoomTypeWeights[type] > roll)
                return type;
        }

        return RoomType.BATTLE;
    }
}
