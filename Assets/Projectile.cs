using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject spawnOrigin;
    public Collider player;
    //float destroyDelay = 10f;

    void Start()
    {
        //Physics.IgnoreLayerCollision(4, 4);
        //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), player);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Collider")
        {
            var drops = spawnOrigin.GetComponent<streamInstance>().waterDrops;
            drops.Remove(gameObject);
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        
    }
}
