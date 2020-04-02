using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamParticle : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public GameObject parent;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void startMovement(Vector3 direction)
    {
        rb.velocity = direction * 6f;
    }

    private void OnDestroy()
    {
        var particles = parent.GetComponent<MilkStream>().particles;
        particles.Remove(gameObject);
    }

    void Update()
    {
        
    }
}
