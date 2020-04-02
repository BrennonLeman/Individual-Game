using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {
    public GameObject stream;
    private List<GameObject> waterObjects = new List<GameObject>();
    float Yrotate = 0;
    float Xrotate = 0;

    void Update() {
        Vector3 direction = transform.forward + transform.position;
        if (Input.GetKeyDown("space"))
        {
            shootStreamInstance(direction);
            print("sjppt"); 
        }
        foreach(GameObject water in waterObjects)
        {
            if(water != null)
            {
                water.transform.position = transform.forward.normalized + transform.position;
                water.transform.rotation = transform.rotation;
            }
        }
        float YturnInput = Input.GetAxisRaw("Horizontal") * 2;
        Yrotate += YturnInput;
        
        float XturnInput = Input.GetAxisRaw("Vertical") * 2;
        Xrotate += XturnInput;

        var newAngle = Quaternion.Euler(Xrotate, Yrotate, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newAngle, Time.deltaTime * 20f);
    }
    void shootStreamInstance(Vector3 Direction)
    {
        GameObject waterDrop = Instantiate(stream, Direction, transform.rotation);
        waterDrop.GetComponent<streamInstance>().player = gameObject.GetComponent<Collider>();
        waterObjects.Add(waterDrop);
    }
}
