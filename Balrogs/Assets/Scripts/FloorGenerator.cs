using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    System.Random randomGenerator = new System.Random();
    //Modifiy accordingly for difficulty or time purposes
    [SerializeField] int roomGridXLength = 8;
    [SerializeField] int roomGridYLength = 8;
    [SerializeField] int nbOfRooms = 15;
    public Room[,] Rooms { get; set; }
    Vector2 FloorSize;

    List<Vector2Int> occupiedRoomPosition;
    List<Vector2Int> nextNeighboors;

    void Start()
    {
        FloorSize = new Vector2(roomGridXLength, roomGridYLength);
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
        Vector2Int currentRoomPosition = GenerateRandomPosition(roomGridXLength, roomGridYLength);
        CreateSingleRoom(currentRoomPosition, RoomType.SPAWN_ROOM);
        occupiedRoomPosition.Add(currentRoomPosition);
        AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());

        for (int i = 0; i < nbOfRooms - 1; i++)
        {
            currentRoomPosition = nextNeighboors[randomGenerator.Next(nextNeighboors.Count)];
            occupiedRoomPosition.Add(currentRoomPosition);
            CreateSingleRoom(currentRoomPosition, RoomType.FLOOR_NORMAL_ROOM);
            nextNeighboors.Remove(currentRoomPosition);
            AddToSelectableNeighboors(Rooms[currentRoomPosition.x, currentRoomPosition.y].GetNeighboors());
        }


    }



    private void SetDoorTypes()
    {
        throw new NotImplementedException();
    }
    private void MiniMapDraw()
    {
        throw new NotImplementedException();
    }




    void Update()
    {
        
    }
}
