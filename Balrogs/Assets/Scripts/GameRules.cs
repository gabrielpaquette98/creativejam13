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
        Difficulty = GameDifficulty.LOW;

    }
    

    // Update is called once per frame
    void Update()
    {
        //Quand le joueur entre en collision avec un tilemap collider de tag "Exit", on sort du niveau pour en faire un nouveau
        //A la génération de terrain, le joueur doit apparaitre au spawn point du prefab de tag Spawn pour le premier niveau, Entry pour les autres levels suivants
        //On doit lier les entrées et sorties (warp zones?)
        
    }
}
