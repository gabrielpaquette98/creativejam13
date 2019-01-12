using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpriteSelector : MonoBehaviour
{
    [SerializeField] Sprite defaultRoom;

    [SerializeField] Color normalColor;
    [SerializeField] Color spawnColor;

    SpriteRenderer roomSpriteRend;
    Room renderedRoom;
    public void setRoom(Room room)
    {
        renderedRoom = room;
    }
    
    void Start()
    {
        roomSpriteRend = GetComponent<SpriteRenderer>();
        PickSprite();
        PickColor();
    }

    void PickColor()
    {
        roomSpriteRend.color = normalColor;
    }

    void PickSprite()
    {
        roomSpriteRend.sprite = defaultRoom;
        SpriteRenderer layerRend;
        List<GameObject> layers = new List<GameObject>();


        for (int i = 0; i < 4; i++)
        {
            layers.Add(transform.GetChild(i).gameObject);
        }
        
        if (!renderedRoom.HasExitUp)
        {
            layerRend = layers[0].GetComponent<SpriteRenderer>();
            layerRend.enabled = false;
        }
        if (!renderedRoom.HasExitRight)
        {
            layerRend = layers[1].GetComponent<SpriteRenderer>();
            layerRend.enabled = false;
        }
        if (!renderedRoom.HasExitDown)
        {
            layerRend = layers[2].GetComponent<SpriteRenderer>();
            layerRend.enabled = false;
        }
        if (!renderedRoom.HasExitLeft)
        {
            layerRend = layers[3].GetComponent<SpriteRenderer>();
            layerRend.enabled = false;
        }
    }
}
