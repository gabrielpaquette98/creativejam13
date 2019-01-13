using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDifficulty
{
    LOW, MEDIUM, HIGH
}

public class GameRules : MonoBehaviour
{
    System.Random randomGenerator = new System.Random();
    public static GameDifficulty Difficulty { get; set; }
    public int PointsGathered { get => pointsGathered; set => pointsGathered = value; }

    [SerializeField] private GameDifficulty initialDifficulty = GameDifficulty.LOW;
    //[SerializeField] int initialDifficulty = 1;
    [SerializeField] int easyRoomCount = 7;
    [SerializeField] int easyMaxOrcCountPerLevel = 4;
    [SerializeField] int easyMaxOrcCountPerRoom = 1;

    [SerializeField] int mediumRoomCount = 13;
    [SerializeField] int mediumMaxOrcCountPerLevel = 10;
    [SerializeField] int mediumMaxOrcCountPerRoom = 2;

    [SerializeField] int hardRoomCount = 17;
    [SerializeField] int hardMaxOrcCountPerLevel = 20;
    [SerializeField] int hardMaxOrcCountPerRoom = 3;

    [SerializeField] bool ennemiesGeneration = true;

    [SerializeField] int roomGridSize = 8;
    const int DEFAULT_ROOM_COUNT = 10;

    private int pointsGathered = 0;
    public int pointsWhenStartingAFloor = 0;

    void Start()
    {
        Difficulty = initialDifficulty;
    }


    void Update()
    {
        //Quand le joueur entre en collision avec un tilemap collider de tag "Exit", on sort du niveau pour en faire un nouveau
        //A la génération de terrain, le joueur doit apparaitre au spawn point du prefab de tag Spawn pour le premier niveau, Entry pour les autres levels suivants
        //On doit lier les entrées et sorties (warp zones?)
        
    }
    public int GetRoomCount()
    {
        switch (Difficulty)
        {
            case GameDifficulty.LOW:
                return easyRoomCount;
            case GameDifficulty.MEDIUM:
                return mediumRoomCount;
            case GameDifficulty.HIGH:
                return hardRoomCount;
        }
        return DEFAULT_ROOM_COUNT;
    }
    public int GetRandomOrcCountPerLevel()
    {
        switch (Difficulty)
        {
            case GameDifficulty.LOW:
                return randomGenerator.Next(easyMaxOrcCountPerLevel-1, easyMaxOrcCountPerLevel+1);
            case GameDifficulty.MEDIUM:
                return randomGenerator.Next(mediumMaxOrcCountPerLevel-2, mediumMaxOrcCountPerLevel+1);
            case GameDifficulty.HIGH:
                return randomGenerator.Next(hardMaxOrcCountPerLevel - 3, hardMaxOrcCountPerLevel + 1);
        }
        return DEFAULT_ROOM_COUNT;
    }
    public int GetRandomOrcCountPerRoom()
    {
        switch (Difficulty)
        {
            case GameDifficulty.LOW:
                return randomGenerator.Next(0, easyMaxOrcCountPerRoom + 1);
            case GameDifficulty.MEDIUM:
                return randomGenerator.Next(0, mediumMaxOrcCountPerRoom + 1);
            case GameDifficulty.HIGH:
                return randomGenerator.Next(0, hardMaxOrcCountPerRoom + 1);
        }
        return DEFAULT_ROOM_COUNT;
    }
    public int GetRoomGridSize()
    {
        return roomGridSize;
    }
    public bool GetEnnemiesGeneration()
    {
        return ennemiesGeneration;
    }
    private void LateUpdate()
    {
        Debug.Log(Difficulty);
    }

    public void DifficultyUpdate()
    {
        switch (Difficulty)
        {
            case GameDifficulty.LOW:
                if (pointsGathered > pointsWhenStartingAFloor)
                    Difficulty = GameDifficulty.MEDIUM;
                break;
            case GameDifficulty.MEDIUM:
                if (pointsGathered > pointsWhenStartingAFloor)
                    Difficulty = GameDifficulty.HIGH;
                else if (pointsGathered < pointsWhenStartingAFloor)
                    Difficulty = GameDifficulty.LOW;
                break;
            case GameDifficulty.HIGH:
                if (pointsGathered < pointsWhenStartingAFloor)
                    Difficulty = GameDifficulty.MEDIUM;
                break;
        }
    }
}
