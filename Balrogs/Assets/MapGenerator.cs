using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private List<Transform> children;

    private GameObject currObj;

    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
            children.Add(child);

        currObj = children[0].gameObject;

    }

    public void NextFloor()
    {
        currObj.active = false;
        i++;
        currObj = children[i].gameObject;
        currObj.active = true;
        
    }

    public void BackToStart()
    {
        Debug.Log("Start");
        currObj.active = false;
        i = 0;
        currObj = children[0].gameObject;
        currObj.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
