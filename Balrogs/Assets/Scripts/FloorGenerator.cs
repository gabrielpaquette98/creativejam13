using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class FloorGenerator : MonoBehaviour
{
    const int GRID_WIDTH = 25;
    const int GRID_HEIGHT = 14;
    const string srcTop = "Prefabs/Maps/Top/";
    const string srcBottom = "Prefabs/Maps/Bottom/";
    const string srcSide = "Prefabs/Maps/Side/";
    System.Random randomGenerator = new System.Random();

    //Modifiy accordingly for difficulty or time purposes
    [SerializeField] int roomArrayXLength = 8;
    [SerializeField] int roomArrayYLength = 8;
    [SerializeField] int nbOfRooms = 10;
    [SerializeField] RoomSpriteSelector roomRenderer;
    [SerializeField] bool miniMapIsVisible = false;
    [SerializeField] bool terrainGenerationIsEnabled = true;
    public Room[,] Rooms { get; set; }
    Vector2 FloorSize;
    GameObject gameGrid;
    List<Vector2Int> occupiedRoomPosition;
    List<Vector2Int> nextNeighboors;

    void Start()
    {
        FloorSize = new Vector2(roomArrayXLength, roomArrayYLength);
        nextNeighboors = new List<Vector2Int>();
        occupiedRoomPosition = new List<Vector2Int>();
        if (nbOfRooms > roomArrayXLength * roomArrayYLength)
            nbOfRooms = roomArrayXLength * roomArrayYLength;
        CreateRooms();
        SetDoorTypes();
        if (miniMapIsVisible)
            MiniMapDraw();
        if (terrainGenerationIsEnabled)
            InitializePlayableRooms();
    }

    void InitializePlayableRooms()
    {
        Room room;
        gameGrid = transform.GetChild(0).gameObject;
        for (int i = 0; i < roomArrayXLength; i++)
        {
            for (int j = 0; j < roomArrayYLength; j++)
            {
                room = Rooms[i,j];
                if (room != null)
                {
                    Vector2 gridRoomPosition = new Vector2(i * GRID_WIDTH, j * GRID_HEIGHT);

                    AddRoomTop(room, gridRoomPosition);
                    AddRoomBottom(room, gridRoomPosition);
                    AddRoomLeft(room, gridRoomPosition);
                    AddRoomRight(room, gridRoomPosition);
                }
            }
        }
    }

    private void AddRoomRight(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "SideMedium";
        if (room.HasExitRight)
        {
            prefabName += "Door";
        }
        GameObject roomRight = Instantiate(Resources.Load(srcSide + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomRight.transform.localScale = new Vector3(roomRight.transform.localScale.x * -1, roomRight.transform.localScale.y, roomRight.transform.localScale.z);
        roomRight.transform.parent = gameGrid.transform;
        roomRight.transform.position = new Vector3(roomRight.transform.position.x - 1, roomRight.transform.position.y, roomRight.transform.position.z);
    }

    private void AddRoomLeft(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "SideMedium";
        if (room.HasExitLeft)
        {
            prefabName += "Door";
        }
        GameObject roomLeft = Instantiate(Resources.Load(srcSide + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomLeft.transform.parent = gameGrid.transform;
    }

    private void AddRoomBottom(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "QuarterBottomMed";
        if (room.HasExitDown)
        {
            prefabName += "Door";
        }
        GameObject roomBottom = Instantiate(Resources.Load(srcBottom + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomBottom.transform.parent = gameGrid.transform;
    }

    private void AddRoomTop(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "QuarterTopMedium";
        if (room.HasExitUp)
        {
            prefabName += "Door";
        }
        GameObject roomTop = Instantiate(Resources.Load(srcTop + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomTop.transform.parent = gameGrid.transform;
    }

    private Vector2Int GenerateRandomPosition(int xMax, int yMax)
    {
        return new Vector2Int(randomGenerator.Next(0, xMax), randomGenerator.Next(0, yMax));
    }
    private void AddToSelectableNeighboors(List<Vector2Int> list)
    {
        foreach (Vector2Int roomPosition in list)
        {
            if (!nextNeighboors.Contains(roomPosition) && !occupiedRoomPosition.Contains(roomPosition))
            {
                nextNeighboors.Add(roomPosition);
            }
        }
    }
    void CreateSingleRoom(Vector2Int position, RoomType type)
    {
        Rooms[position.x, position.y] = new Room(position, type);
    }
    private void CreateRooms()
    {
        Rooms = new Room[roomArrayXLength, roomArrayYLength];
        Vector2Int currentRoomPosition = new Vector2Int(roomArrayXLength/2, roomArrayYLength/2);//GenerateRandomPosition(roomGridXLength, roomGridYLength);
        CreateSingleRoom(currentRoomPosition, RoomType.SPAWN_ROOM);
        occupiedRoomPosition.Add(currentRoomPosition);
        AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());

        for (int i = 0; i < nbOfRooms - 1; i++)
        {
            if (i >= nbOfRooms - nbOfRooms/5)
            {
                currentRoomPosition = ChooseNextRoomRandomly();
            }
            else
            {
                currentRoomPosition = ChooseNextBranchRoom();
            }
            
            occupiedRoomPosition.Add(currentRoomPosition);
            CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_NORMAL_ROOM);
            nextNeighboors.Remove(currentRoomPosition);
            AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());
        }
    }

    Vector2Int ChooseNextBranchRoom()
    {
        //Pick rooms with at most 1 occupied neighboor space
        List<Vector2Int> roomsToTry = new List<Vector2Int>();
        foreach (var item in nextNeighboors)
        {
            roomsToTry.Add(item);
        }
        Vector2Int position = new Vector2Int();
        do
        {
            position = roomsToTry[randomGenerator.Next(0, roomsToTry.Count)];
            roomsToTry.Remove(position);
        } while (MakeListOfOccupiedNeighboors(position).Count > 1 && roomsToTry.Count > 0);

        return position;
    }

    private List<Vector2Int> MakeListOfOccupiedNeighboors(Vector2Int position)
    {
        List<Vector2Int> possibleNeighboors = Room.GetNeighboors(position);
        List<Vector2Int> occupiedNeighboors = new List<Vector2Int>();
        foreach (Vector2Int roomPosition in possibleNeighboors)
        {
            if (occupiedRoomPosition.Contains(roomPosition))
            {
                occupiedNeighboors.Add(roomPosition);
            }
        }
        return occupiedNeighboors;
    }

    Vector2Int ChooseNextRoomRandomly()
    {
        return nextNeighboors[randomGenerator.Next(0, nextNeighboors.Count)];
    }

    void SetDoorTypes()
    {
        foreach (Room room in Rooms)
        {
            if (room != null)
            {
                List<Vector2Int> neighboors = MakeListOfOccupiedNeighboors(room.GridPosition);
                foreach (Vector2Int neighbor in neighboors)
                {
                    if (neighbor.y - room.GridPosition.y == -1)
                    {
                        room.HasExitDown = true;
                    }
                    if (neighbor.x - room.GridPosition.x == 1)
                    {
                        room.HasExitRight = true;
                    }
                    if (neighbor.y - room.GridPosition.y == 1)
                    {
                        room.HasExitUp = true;
                    }
                    if (neighbor.x - room.GridPosition.x == -1)
                    {
                        room.HasExitLeft = true;
                    }
                }
            }
        }
    }
    void MiniMapDraw()
    {
        foreach (Room room in Rooms)
        {
            if (room != null)
            {
                Vector2 drawPosition = room.GridPosition;
                drawPosition.x /= 6;
                drawPosition.y /= 12;
                GameObject roomObj = Instantiate(roomRenderer, drawPosition, Quaternion.identity).gameObject;
                RoomSpriteSelector roomScript = roomObj.GetComponent<RoomSpriteSelector>();
                roomScript.setRoom(room);

            }
        }
    }




    void Update()
    {
        
    }
}
