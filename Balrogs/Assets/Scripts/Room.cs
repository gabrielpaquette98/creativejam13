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
    RoomType CurrentRoomType { get; set; }
    bool[] ExitDirections { get; set; }

    public Room(Vector2Int position, RoomType roomType) {
        GridPosition = position;
        CurrentRoomType = roomType;
    }
    public List<Vector2Int> GetNeighboors()
    {
        return GetNeighboors(GridPosition);
    }
    public List<Vector2Int> GetNeighboors(Vector2Int roomPosition)
    {
        List<Vector2Int> neighboors = new List<Vector2Int>();
        if (GridPosition.x > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x - 1, roomPosition.y));
        }
        if (GridPosition.y > 0)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y - 1));
        }
        if (GridPosition.x < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x + 1, roomPosition.y));
        }
        if (GridPosition.y < 7)
        {
            neighboors.Add(new Vector2Int(roomPosition.x, roomPosition.y + 1));
        }
        return neighboors;
    }
}
