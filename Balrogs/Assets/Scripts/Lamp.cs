using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RockShot") || other.gameObject.CompareTag("Rock"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.GetComponentInChildren<Light>().enabled = false;
            gameObject.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
    }
}
