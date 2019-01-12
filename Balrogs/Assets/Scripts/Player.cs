using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float PLAYER_SPEED = 2f;
    private float speed;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        speed = PLAYER_SPEED;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetKeyboardInput();
        Move();
    }

    public void Move() {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void GetKeyboardInput() {
        direction = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector2.up;
        } 
        if (Input.GetKey(KeyCode.S)) {
            direction += Vector2.down;
        } 
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector2.left;
        } 
        if (Input.GetKey(KeyCode.D)) {
            direction += Vector2.right;
        } 
        if (Input.GetKey(KeyCode.Space)) {    
            // Throws rock?
        }
    }
}
