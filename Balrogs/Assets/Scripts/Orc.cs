﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private const float EPSILON = 0.05f;

    private bool chasing = false, stun = false;

    private float timer = 0.0f;
    
    enum States {LOOKING, CHASING, PARTOL, STUN}

    [SerializeField]
    private States state = States.PARTOL;

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
    
    bool ComparePos(Vector3 target)
    {
        return (transform.position.x < target.x + EPSILON && transform.position.x > target.x - EPSILON &&
                transform.position.y < target.y + EPSILON && transform.position.y > target.y - EPSILON);
    }

    IEnumerator lookForPlayer()
    {
        

        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(1);
            transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
        }

        state = States.PARTOL;
    }

    private void patrol()
    {
        //float step = speed * Time.deltaTime;

        Vector3 dirr = target - transform.position;

        GetComponent<Rigidbody2D>().velocity = dirr.normalized * speed;
        
        LayerMask layerMask = (LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirr,Mathf.Infinity, 1 <<LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position+dirr.normalized, dirr, Color.white, 0.5f, true);


        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag.Equals("Player"))
            {
                //target = hit.collider.transform.position;
                speed *= 2;

                state = States.CHASING;
                StopAllCoroutines();
                target = hit.collider.transform.position;

            }
            
        } 
        if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            transform.localScale = new Vector3(-1,1,1);
        }
    }

    private void Chasing()
    {
        Debug.Log("CHASE");
            Vector3 dirr = target - transform.position;

            GetComponent<Rigidbody2D>().velocity = dirr.normalized * speed;
            
            Debug.DrawRay(transform.position+dirr.normalized, dirr, Color.white, 0.5f, true);
            
            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1,1,1);
            }

        

        if (ComparePos())
        {
            speed /= 2;
            state = States.LOOKING;
            StopAllCoroutines();
            StartCoroutine(lookForPlayer());
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }

   

    private void looking()
    {
        Vector3 dirr = transform.position;
        dirr.x += 30*transform.localScale.x;
        Debug.Log(dirr + "\n" + transform.position);
        
        LayerMask layerMask = (LayerMask.GetMask("Player"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirr,Mathf.Infinity, 1 <<LayerMask.NameToLayer("Player"));
        Debug.DrawRay(transform.position, dirr, Color.white, 0.5f, true);

        //If something was hit, the RaycastHit2D.collider will not be null.
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag.Equals("Player"))
            {
                //target = hit.collider.transform.position;
                speed *= 2;
                state = States.CHASING;
                StopAllCoroutines();
                target = hit.collider.transform.position;

            }
            
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
        if (ComparePos() && state == States.PARTOL)
        {
            index++;
            if (index == path.Length) index = 0;
            target = path[index].transform.position;
        }
        
        switch (state)
        {
            case States.LOOKING : looking();
                break;
            case States.STUN : break;
            case States.PARTOL : patrol();
                break;
            case States.CHASING : Chasing();
                break;

        }
        

        //float step = speed * Time.deltaTime;
        Vector3 dirr = target - transform.position;
       //RaycastHit2D hit = Physics2D.Raycast(transform.position+dirr.normalized, dirr);
       

        Debug.Log(state);
        


        // Move our position a step closer to the target.
        //transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
