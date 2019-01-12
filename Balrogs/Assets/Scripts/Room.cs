using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomType
{
    FLOOR_START_ROOM, FLOOR_NORMAL_ROOM, FLOOR_EXIT_ROOM, SPAWN_ROOM
}

public class Room
{
    Vector2Int GridPosition { get; set; }
    RoomType CurrentRoomType { get; set; }
    bool[] ExitDirections { get; set; }

    public Room(Vector2Int position, RoomType roomType) {
        GridPosition = position;
        CurrentRoomType = roomType;
    }
    public List<Vector2Int> GetNeighboors()
    {
        List<Vector2Int> neighboors = new List<Vector2Int>();
        if (GridPosition.x > 0)
        {
            neighboors.Add(new Vector2Int(GridPosition.x - 1, GridPosition.y));
        }
        if (GridPosition.y > 0)
        {
            neighboors.Add(new Vector2Int(GridPosition.x, GridPosition.y - 1));
        }
        if (GridPosition.x < 8)
        {
            neighboors.Add(new Vector2Int(GridPosition.x + 1, GridPosition.y));
        }
        if (GridPosition.y < 8)
        {
            neighboors.Add(new Vector2Int(GridPosition.x, GridPosition.y + 1));
        }
        return neighboors;
    }

}
