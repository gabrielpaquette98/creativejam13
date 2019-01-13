using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowRock : Poolable
{
    private Rigidbody2D rigidBody;
    private GameObject player;
    [SerializeField]
    private float speed;
    private float lifeTime;
    public AudioSource inpact, shoot;

    void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //rigidBody.drag = 1;
        player = GameObject.Find("Player");
        //speed = 15f;
        ShootRock();
        lifeTime = 10f;
        //Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void OnCollisionEnter(Collision other)
    {
        inpact.Play();
    }

    private void ShootRock()
    {
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosition = Input.mousePosition;
        Vector2 direction = ((Vector2)mousePosition - playerPosition).normalized;
        rigidBody.AddForce(direction * speed, ForceMode2D.Impulse);
        shoot.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!(other.gameObject.CompareTag("Player")) && !(other.gameObject.CompareTag("RockShot"))) {
            gameObject.tag = "Rock";
            this.enabled = false;
        }
        else
        {
            this.GetComponents<Collider2D>()[1].isTrigger = false;
        }
    }
}
