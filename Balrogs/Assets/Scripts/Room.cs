using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomType
{
    FLOOR_START_ROOM, FLOOR_NORMAL_ROOM, FLOOR_EXIT_ROOM, SPAWN_ROOM
}

public class Room
{
    public Vector2Int GridPosition { get; private set; }
    public RoomType CurrentRoomType { get; private set; }
    bool[] ExitDirections { get; set; }

    public bool HasExitUp { get; set; }
    public bool HasExitRight { get; set; }
    public bool HasExitDown { get; set; }
    public bool HasExitLeft { get; set; }

    bool isSpawn;

    public Room(Vector2Int position, RoomType roomType) {
        GridPosition = position;
        CurrentRoomType = roomType;

        HasExitUp = false;
        HasExitDown = false;
        HasExitLeft = false;
        HasExitRight = false;
        
}
    public List<Vector2Int> GetNeighboors()
    {
        return GetNeighboors(GridPosition);
    }

    public static List<Vector2Int> GetNeighboors(Vector2Int roomPosition)
    {
        List<Vector2Int> neighboors = new List<Vector2Int>();
        if (roomPosition.y > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y - 1));
        }
        if (roomPosition.x > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x - 1, roomPosition.y));
        }
        if (roomPosition.x < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x + 1, roomPosition.y));
        }
        if (roomPosition.y < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y + 1));
        }
        return neighboors;
    }
    public static List<Vector2Int> GetNeighboorsButNoTop(Vector2Int roomPosition)
    {
        List<Vector2Int> neighboors = new List<Vector2Int>();
        if (roomPosition.y > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y - 1));
        }
        if (roomPosition.x > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x - 1, roomPosition.y));
        }
        if (roomPosition.x < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x + 1, roomPosition.y));
        }
        return neighboors;
    }
    public static List<Vector2Int> GetNeighboorsButNoBottom(Vector2Int roomPosition)
    {
        List<Vector2Int> neighboors = new List<Vector2Int>();
        if (roomPosition.x > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x - 1, roomPosition.y));
        }
        if (roomPosition.x < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x + 1, roomPosition.y));
        }
        if (roomPosition.y < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y + 1));
        }
        return neighboors;
    }
}
