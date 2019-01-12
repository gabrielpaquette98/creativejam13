using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : MonoBehaviour
{
    [SerializeField]
    private GameObject[] path;

    [SerializeField]
    private Vector3 target;

    [SerializeField]
    private int index = 0;

    [SerializeField]
    private float speed = 1.0f;

    private const float EPSILON = 0.0005f;

    // Start is called before the first frame update
    void Start()
    {
        path = GameObject.FindGameObjectsWithTag("path");
        //thisPos = transform.position;
        target = path[index].transform.position;
    }

    bool ComparePos()
    {
        return (transform.position.x < target.x + EPSILON && transform.position.x > target.x - EPSILON &&
                transform.position.y < target.y + EPSILON && transform.position.y > target.y - EPSILON);
    }

    // Update is called once per frame
    void Update()
    {
        if (ComparePos())
        {
            index++;
            if (index == 4) index = 0;
            target = path[index].transform.position;
        }

        //float step = speed * Time.deltaTime;

        Vector3 dirr = target - transform.position;

        GetComponent<Rigidbody2D>().velocity = dirr.normalized * speed;

        if (dirr.x < 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        // Move our position a step closer to the target.
        //transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
