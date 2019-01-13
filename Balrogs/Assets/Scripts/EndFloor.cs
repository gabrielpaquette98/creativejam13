using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class EndFloor : MonoBehaviour
{
    const int GRID_WIDTH = 25;
    const int GRID_HEIGHT = 14;
    const string SRC_TOP = "Prefabs/Maps/Top/";
    const string SRC_BOTTOM = "Prefabs/Maps/Bottom/";
    const string SRC_SIDE = "Prefabs/Maps/Side/";

    public GameRules Rules { get; private set; }
    public Room[,] Rooms { get; private set; }

    int roomArrayXLength;
    int roomArrayYLength;

    [SerializeField] RoomSpriteSelector roomRenderer;

    public GameObject player;
    GameObject gameGrid;

    void OnEnable()
    {
        Rules = GameObject.FindGameObjectWithTag("Rules").GetComponent<GameRules>();
        roomArrayXLength = Rules.GetRoomGridSize();
        roomArrayYLength = Rules.GetRoomGridSize();
        CreateRooms();
        InitializePlayableRooms();
        Rules.pointsWhenStartingAFloor = Rules.PointsGathered;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn") as GameObject;
        if (spawn == null)
            spawn = GameObject.FindGameObjectWithTag("Entry") as GameObject;
        if (player == null)
        {
            player = Instantiate(this.player);
        }
        player.transform.position = spawn.transform.position;
    }

    void InitializePlayableRooms()
    {
        AddRoomBottom();
    }
    private void AddRoomBottom()
    {
        string prefabName = "EnderBottom";
        GameObject roomBottom = Instantiate(Resources.Load(SRC_BOTTOM + prefabName), Vector2.one, Quaternion.identity) as GameObject;
        roomBottom.transform.parent = gameGrid.transform;
    }
    void CreateSingleRoom(Vector2Int position, RoomType type)
    {
        Rooms[position.x, position.y] = new Room(position, type);
    }
    private void CreateRooms()
    {
        Rooms = new Room[roomArrayXLength, roomArrayYLength];
        Vector2Int currentRoomPosition = new Vector2Int(roomArrayXLength / 2, roomArrayYLength / 2);
        CreateSingleRoom(currentRoomPosition, RoomType.SPAWN_ROOM);
        gameGrid = transform.GetChild(0).gameObject;
    }
}
