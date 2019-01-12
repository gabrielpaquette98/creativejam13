using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDifficulty
{
    LOW, MEDIUM, HIGH
}

public class GameRules : MonoBehaviour
{
    public static GameDifficulty Difficulty { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Difficulty = GameDifficulty.MEDIUM;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
