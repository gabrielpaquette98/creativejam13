﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject rock;
    public Transform throwPoint;
    public Text rockCountUI;
    private Rigidbody2D rigidBody;
    private float horizontal;
    private float vertical;
    private float limit;
    private float speed;
    public int rockCount;
    bool hasCollided = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        limit = 0.8f;
        speed = 5f;
        rockCount = 0;
        UpdateRockCountUI();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        ThrowRock();
    }

    void FixedUpdate()
    {
        rigidBody.velocity = (horizontal != 0 && vertical != 0) ? new Vector2((horizontal * speed) * limit, (vertical * speed) * limit) :
                                                                  new Vector2(horizontal * speed, vertical * speed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasCollided && other.gameObject.CompareTag("Rock"))
        {
            hasCollided = true;
            rockCount++;
            UpdateRockCountUI();
            other.gameObject.SetActive(false);
        } else if (hasCollided) { hasCollided = false; }
    }

    private void UpdateRockCountUI()
    {
        rockCountUI.text = "x  " + rockCount;
    }
    private void ThrowRock()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && rockCount != 0)
        {
            Instantiate(rock, throwPoint.position, throwPoint.rotation);
            rockCount--;
            UpdateRockCountUI();
        }
    }
}
