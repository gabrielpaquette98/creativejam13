using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class FloorGenerator : MonoBehaviour
{
    System.Random randomGenerator = new System.Random();
    //Modifiy accordingly for difficulty or time purposes
    [SerializeField] int roomGridXLength = 8;
    [SerializeField] int roomGridYLength = 8;
    [SerializeField] int nbOfRooms = 10;
    [SerializeField] RoomSpriteSelector roomRenderer;
    public Room[,] Rooms { get; set; }
    Vector2 FloorSize;

    List<Vector2Int> occupiedRoomPosition;
    List<Vector2Int> nextNeighboors;

    void Start()
    {
        FloorSize = new Vector2(roomGridXLength, roomGridYLength);
        nextNeighboors = new List<Vector2Int>();
        occupiedRoomPosition = new List<Vector2Int>();
        if (nbOfRooms > roomGridXLength * roomGridYLength)
            nbOfRooms = roomGridXLength * roomGridYLength;
        CreateRooms();
        SetDoorTypes();
        MiniMapDraw();
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
        Rooms = new Room[roomGridXLength, roomGridYLength];
        Vector2Int currentRoomPosition = new Vector2Int(roomGridXLength/2, roomGridYLength/2);//GenerateRandomPosition(roomGridXLength, roomGridYLength);
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
