using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    const string PoolKey = "Coin.prefab";
    [SerializeField] GameObject prefab;
    List<Poolable> instances = new List<Poolable>();

    // Start is called before the first frame update
    void Start()
    {
        if (GameObjectPoolController.AddEntry(PoolKey, prefab, 10, 15))
            Debug.Log("Pre-populating pool");
        else
            Debug.Log("Pool already configured");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("RockShot") || other.gameObject.CompareTag("Rock"))
        {
            int coinAmmount = Random.Range(1, 3);
            for (int i = 0; i < coinAmmount; i++)
            {
                Poolable obj = GameObjectPoolController.Dequeue(PoolKey);
                obj.transform.position = transform.position;
                obj.gameObject.SetActive(true);
                instances.Add(obj);
                Vector3 otherSpeed = other.GetComponent<Rigidbody2D>().velocity;
                obj.GetComponent<Rigidbody2D>().velocity = otherSpeed / 2;
            }

            gameObject.active = false;

        }
    }
}
