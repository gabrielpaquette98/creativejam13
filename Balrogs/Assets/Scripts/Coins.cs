using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : Poolable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(gameObject.tag);
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<Player>().Coins++;
            GetComponent<AudioSource>().Play();
            StartCoroutine(disableCoin());


        }
    }

    IEnumerator disableCoin()
    {
        yield return new WaitForSeconds(0.07f);
        gameObject.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
