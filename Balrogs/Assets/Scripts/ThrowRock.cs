using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    const float ROCK_SPEED = 10f;
    private float speed;
    private Rigidbody2D rigidBody;
    public Vector2 facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        speed = ROCK_SPEED;
        facingDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GetDirection();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveRock();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void MoveRock()
    {
        rigidBody.velocity = facingDirection * speed;
    }
    private void GetDirection()
    {
        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))) {}
        else {
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector2.right;
            }
            facingDirection = direction;
        }
    }
}
