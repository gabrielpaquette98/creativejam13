using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private GameObject player;
    private float speed;
    private float lifeTime;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.drag = 1;
        player = GameObject.Find("Player");
        speed = 10f;
        ShootRock();
        lifeTime = 10f;
    }

    private void ShootRock()
    {
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = Input.mousePosition;
        Vector2 direction = ((Vector2)mousePosition - playerPosition).normalized;
        rigidBody.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!(other.gameObject.CompareTag("Player")) && !(other.gameObject.CompareTag("RockShot"))) {
            gameObject.tag = "Rock";
            Destroy(gameObject, lifeTime);
        }
    }
}
