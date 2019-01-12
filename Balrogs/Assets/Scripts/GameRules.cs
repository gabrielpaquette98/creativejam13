using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDifficulty
{
    LOW, MEDIUM, HIGH
}

public class GameRules : MonoBehaviour
{
    public GameDifficulty Difficulty { get; private set; } = GameDifficulty.LOW;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
