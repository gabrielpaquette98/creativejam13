using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject rock;
    public Transform throwPoint;

    private Rigidbody2D rigidBody;
    private float horizontal;
    private float vertical;
    private float limit;
    private float speed;
    private int rockCount;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        limit = 0.5f;
        speed = 5f;
        rockCount = 0;
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
        if (other.gameObject.CompareTag("Rock"))
        {
            rockCount++;
            other.gameObject.SetActive(false);
        }
    }

    private void ThrowRock()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && rockCount != 0)
        {
            Instantiate(rock, throwPoint.position, throwPoint.rotation);
            rockCount--;
        }
    }
}
