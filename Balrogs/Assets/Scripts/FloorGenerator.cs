using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    static bool firstLevelDone = false;
    
    const int GRID_WIDTH = 25;
    const int GRID_HEIGHT = 14;
    const string SRC_TOP = "Prefabs/Maps/Top/";
    const string SRC_BOTTOM = "Prefabs/Maps/Bottom/";
    const string SRC_SIDE = "Prefabs/Maps/Side/";

    const string EASY = "Ez";
    const string MEDIUM = "Medium";
    const string HARD = "Hard";
    string difficulty;

    System.Random randomGenerator = new System.Random();

    //Modifiy Rules accordingly for difficulty or time purposes
    GameRules Rules;
    int roomArrayXLength;
    int roomArrayYLength;

    int NbOfRooms => Rules.GetRoomCount();
    [SerializeField] RoomSpriteSelector roomRenderer;
    [SerializeField] bool miniMapIsVisible = false;
    [SerializeField] bool terrainGenerationIsEnabled = true;

    public Room[,] Rooms { get; set; }
    Vector2 FloorSize;
    GameObject gameGrid;
    List<Vector2Int> occupiedRoomPosition;
    List<Vector2Int> nextNeighboors;
    
    public GameObject player;
    [SerializeField] GameObject ennemyPrefab;

    public bool firstTime = true;
    int numberOfEnnemiesSpawned = 0;
    int maxNumberOfEnnemies = 0;

    void OnEnable()
    {
        Rules = GameObject.FindGameObjectWithTag("Rules").GetComponent<GameRules>();
        roomArrayXLength = Rules.GetRoomGridSize();
        roomArrayYLength = Rules.GetRoomGridSize();
        
        if (firstTime)
        {
            FloorSize = new Vector2(roomArrayXLength, roomArrayYLength);
            nextNeighboors = new List<Vector2Int>();
            occupiedRoomPosition = new List<Vector2Int>();
            CreateRooms();
            SetDoorTypes();
            if (miniMapIsVisible)
                MiniMapDraw();
            if (terrainGenerationIsEnabled)
                InitializePlayableRooms();
            if (Rules.GetEnnemiesGeneration())
                SprinkleEnnemiesInLevel();
            Rules.pointsWhenStartingAFloor = Rules.PointsGathered;
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn") as GameObject;
        if(spawn==null)
            spawn = GameObject.FindGameObjectWithTag("Entry") as GameObject;
        if ( player == null)
        {
            player = Instantiate(this.player);
        }

        player.transform.position = spawn.transform.position;
        firstTime = false;

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

    private void SprinkleEnnemiesInLevel()
    {
        Vector2Int roomPosition;
        maxNumberOfEnnemies = Rules.GetRandomOrcCountPerLevel();
        List<Vector2Int> roomsToSprinkle = new List<Vector2Int>();
        foreach (var room in occupiedRoomPosition)
        {
            if (Rooms[room.x, room.y] != null && Rooms[room.x, room.y].CurrentRoomType != RoomType.SPAWN_ROOM)
                roomsToSprinkle.Add(room);
        }
        do
        {
            
            roomPosition = roomsToSprinkle[randomGenerator.Next(0, roomsToSprinkle.Count)];
            SprinkleEnnemiesInRoom(roomPosition);
            roomsToSprinkle.Remove(roomPosition);
        } while (numberOfEnnemiesSpawned < maxNumberOfEnnemies && roomsToSprinkle.Count > 0);

    }

    private void SprinkleEnnemiesInRoom(Vector2Int roomPosition)
    {
        int amountOfEnnemiesToSpawn = Rules.GetRandomOrcCountPerRoom();
        if (numberOfEnnemiesSpawned + amountOfEnnemiesToSpawn > maxNumberOfEnnemies)
        {
            amountOfEnnemiesToSpawn = maxNumberOfEnnemies - numberOfEnnemiesSpawned;
        }
        for (int i = 0; i < amountOfEnnemiesToSpawn; i++)
        {
            SpawnEnnemy(roomPosition);
        }
    }

    void SpawnEnnemy(Vector2 roomPosition)
    {
        Vector2 spawnPosition = new Vector2(roomPosition.x * GRID_WIDTH, roomPosition.y * GRID_HEIGHT);
        GameObject ennemy = Instantiate(ennemyPrefab, spawnPosition, Quaternion.identity);

        //set path
        Orc orc = ennemy.GetComponent<Orc>();
        orc.initialisePath(spawnPosition);
        numberOfEnnemiesSpawned++;
    }

    private void AddRoomRight(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "Side" + ChooseQuarterDifficulty();
        if (room.HasExitRight)
        {
            prefabName += "Door";
        }
        GameObject roomRight = Instantiate(Resources.Load(SRC_SIDE + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomRight.transform.localScale = new Vector3(roomRight.transform.localScale.x * -1, roomRight.transform.localScale.y, roomRight.transform.localScale.z);
        roomRight.transform.parent = gameGrid.transform;
        roomRight.transform.position = new Vector3(roomRight.transform.position.x - 1, roomRight.transform.position.y, roomRight.transform.position.z);
    }


    private void AddRoomLeft(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "Side" + ChooseQuarterDifficulty();
        if (room.HasExitLeft)
        {
            prefabName += "Door";
        }
        GameObject roomLeft = Instantiate(Resources.Load(SRC_SIDE + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomLeft.transform.parent = gameGrid.transform;
    }

    private void AddRoomBottom(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "QuarterBottom";
        if (room.CurrentRoomType == RoomType.FLOOR_START_ROOM)
        {
            prefabName += "Start";
        }
        else
        {
            if (room.HasExitDown)
            {
                prefabName += ChooseQuarterDifficulty();
                prefabName += "Door";
            }
            else
            {
                int randomSelector = randomGenerator.Next(0, 2);
                if (randomSelector == 1)
                {
                    prefabName += ChooseQuarterDifficulty();
                }
                else
                {
                    prefabName += "Small";
                }
            }
        }

        GameObject roomBottom = Instantiate(Resources.Load(SRC_BOTTOM + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        roomBottom.transform.parent = gameGrid.transform;
    }

    private void AddRoomTop(Room room, Vector2 gridRoomPosition)
    {
        string prefabName = "QuarterTop";
        if (room.CurrentRoomType == RoomType.SPAWN_ROOM)
        {
            prefabName += "Spawn";
        }
        else if (room.CurrentRoomType == RoomType.FLOOR_EXIT_ROOM)
        {
            prefabName += "Exit";
        }
        else
        {
            int randomSelector = randomGenerator.Next(0, 2);
            if (randomSelector == 1)
            {
                prefabName += "Smaller";
            }
            prefabName += ChooseQuarterDifficulty();
            if (room.HasExitUp)
                prefabName += "Door";
        }
        GameObject roomTop = Instantiate(Resources.Load(SRC_TOP + prefabName), gridRoomPosition, Quaternion.identity) as GameObject;
        /*if (room.CurrentRoomType == RoomType.SPAWN_ROOM)
        {
            GameObject spawn = GameObject.FindGameObjectWithTag("Spawn") as GameObject;
            Instantiate(player, spawn.transform.position, spawn.transform.rotation);
        }*/

        roomTop.transform.parent = gameGrid.transform;
    }
    
    private string ChooseQuarterDifficulty()
    {
        int randomSelector = randomGenerator.Next(0, 3);

        switch (GameRules.Difficulty)
        {
            case GameDifficulty.LOW:
                if (randomSelector <= 1)
                    return EASY;
                else
                    return MEDIUM;
                break;
            case GameDifficulty.MEDIUM:
                if (randomSelector == 0)
                    return EASY;
                if (randomSelector == 1)
                    return MEDIUM;
                if (randomSelector == 2)
                    return HARD;
                break;
            case GameDifficulty.HIGH:
                    return HARD;
                break;
            default:
                return MEDIUM;
                break;
        }
        return MEDIUM;
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
        Vector2Int currentRoomPosition = new Vector2Int(roomArrayXLength/2, roomArrayYLength/2);
        if (!firstLevelDone)
        {
            CreateSingleRoom(currentRoomPosition, RoomType.SPAWN_ROOM);
            firstLevelDone = true;
            occupiedRoomPosition.Add(new Vector2Int(roomArrayXLength / 2, roomArrayYLength / 2 + 1));
        }
        else
        {
            CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_START_ROOM);
            occupiedRoomPosition.Add(new Vector2Int(roomArrayXLength / 2, roomArrayYLength / 2 - 1));
        }
        
        AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());
        occupiedRoomPosition.Add(currentRoomPosition);
        

        for (int i = 0; i < NbOfRooms - 1; i++)
        {
            if (i > NbOfRooms - NbOfRooms/5)
            {
                currentRoomPosition = ChooseNextRoomRandomly();
                CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_NORMAL_ROOM);
                occupiedRoomPosition.Add(currentRoomPosition);
                nextNeighboors.Remove(currentRoomPosition);
                AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());
            }
            else if (i == NbOfRooms - NbOfRooms / 5 )
            {
                Vector2Int topPosition; 
                do
                {
                    currentRoomPosition = ChooseNextBranchRoom();
                    topPosition = new Vector2Int(currentRoomPosition.x, currentRoomPosition.y + 1);
                } while (occupiedRoomPosition.Contains(topPosition));
                CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_EXIT_ROOM);
                occupiedRoomPosition.Add(currentRoomPosition);
                occupiedRoomPosition.Add(new Vector2Int(currentRoomPosition.x, currentRoomPosition.y + 1));
                nextNeighboors.Remove(currentRoomPosition);
                AddToSelectableNeighboors(Room.GetNeighboorsButNoTop(currentRoomPosition));
            }
            else
            {
                currentRoomPosition = ChooseNextBranchRoom();
                CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_NORMAL_ROOM);
                occupiedRoomPosition.Add(currentRoomPosition);
                nextNeighboors.Remove(currentRoomPosition);
                AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());
            }
        }
    }

    Vector2Int ChooseNextBranchRoom()
    {
        //Pick rooms with at most 1 occupied neighboor space
        List<Vector2Int> roomsToTry = new List<Vector2Int>();
        foreach (var item in nextNeighboors)
        {
            if (!occupiedRoomPosition.Contains(item))
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
        Vector2Int nextPosition = new Vector2Int();
        do
        {
            nextPosition = nextNeighboors[randomGenerator.Next(0, nextNeighboors.Count)];
        } while (occupiedRoomPosition.Contains(nextPosition));
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
                    if (Rooms[neighbor.x, neighbor.y] != null)
                    {
                        if (Rooms[neighbor.x, neighbor.y].CurrentRoomType != RoomType.SPAWN_ROOM && neighbor.y - room.GridPosition.y == -1)
                        {
                            room.HasExitDown = true;
                        }
                        if (neighbor.x - room.GridPosition.x == 1)
                        {
                            room.HasExitRight = true;
                        }
                        if (neighbor.y - room.GridPosition.y == 1 && room.CurrentRoomType != RoomType.SPAWN_ROOM)
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

    
}
