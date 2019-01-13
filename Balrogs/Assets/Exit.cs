using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private GameObject Generator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        Generator = GameObject.FindGameObjectWithTag("MapGenerator");
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Generator.GetComponent<MapGenerator>().NextFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
