using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite defaultRoom;
    [SerializeField] Color normalColor;
    [SerializeField] Color spawnColor;

    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        PickSprite();
        PickColor();
    }

    void PickColor()
    {
        rend.color = normalColor;
    }

    void PickSprite()
    {
        rend.sprite = defaultRoom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
