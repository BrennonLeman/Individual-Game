using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MilkStream : MonoBehaviour
{
    private float fireRate = 30f;
    private float timeToNextShot = 0f;
    private bool canShoot = true;
    private LineRenderer stream;
    private float streamWidth = 0.5f;
    private Vector3 directionStartPosition;
    private Utter utter;

    [HideInInspector] public List<GameObject> particles = new List<GameObject>();

    [SerializeField] private Material _material;

    [SerializeField] private GameObject particle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        utter = transform.parent.parent.gameObject.GetComponent<Utter>();
        directionStartPosition = utter.midPoint;
        stream = gameObject.AddComponent<LineRenderer>();
        stream.startWidth = streamWidth;
        stream.endWidth = streamWidth;
        stream.material = _material;
        stream.numCapVertices = 16;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = transform.position - directionStartPosition;
        if (Time.time >= timeToNextShot && canShoot  && utter.canShoot)
        {
            timeToNextShot = Time.time + 1f / fireRate;
            shootStream(direction);
        }
        drawStream();
        if (!utter.canShoot)
        {
            foreach(GameObject sP in particles)
            {
                Destroy(sP, 2f);
            }
            canShoot = false;
        }
        if(!canShoot && particles.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void shootStream(Vector3 direction)
    {
        GameObject streamParticle = Instantiate(particle, transform.position + transform.forward * .4f, Quaternion.identity);
        streamParticle.GetComponent<StreamParticle>().startMovement(direction);
        streamParticle.GetComponent<StreamParticle>().parent = gameObject;
        particles.Add(streamParticle);
    }

    private void drawStream()
    {
        stream.positionCount = particles.Count;
        var points = new Vector3[stream.positionCount];
        for (int i = 0; i < particles.Count; i++)
        {
            points[i] = particles[i].transform.position;
        }
        stream.SetPositions(points);
    }
}
