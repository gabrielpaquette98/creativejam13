using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private GameObject player;
    private float speed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        speed = 10f;
        ShootRock();
    }

    private void ShootRock()
    {
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = Input.mousePosition;
        Vector2 direction = ((Vector2)mousePosition - playerPosition).normalized;
        rigidBody.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.CompareTag("Player")))
        {
            Destroy(gameObject);
        }
    }
}
