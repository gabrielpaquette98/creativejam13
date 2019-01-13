﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject rock;
    public Transform throwPoint;
    
    const string PoolKey = "RockShot.prefab";
    [SerializeField] GameObject prefab;
    List<Poolable> instances = new List<Poolable>();

    private Rigidbody2D rigidBody;
    private float horizontal;
    private float vertical;
    private float limit;
    private float speed;
    public int rockCount;
    bool hasCollided = false;

    [SerializeField]
    private bool illuminated = false;

    public bool Illuminated
    {
        get { return illuminated; }
        set { illuminated = value; }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        limit = 0.8f;
        speed = 5f;
        rockCount = 0;
        
        if (GameObjectPoolController.AddEntry(PoolKey, prefab, 10, 15))
            Debug.Log("Pre-populating pool");
        else
            Debug.Log("Pool already configured");
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        ThrowRock();
    }

    void FixedUpdate()
    {
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        rigidBody.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCollided && other.gameObject.CompareTag("Rock"))
        {
            hasCollided = true;
            rockCount++;
            other.gameObject.SetActive(false);
        } else if (hasCollided) { hasCollided = false; }
    }

    private void ThrowRock()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && rockCount != 0)
        {
            //Instantiate(rock, throwPoint.position, throwPoint.rotation);
            
            Poolable obj = GameObjectPoolController.Dequeue(PoolKey);
            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
            instances.Add(obj);
            
            rockCount--;
        }
    }
}
