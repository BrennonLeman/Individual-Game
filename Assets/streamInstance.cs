using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class streamInstance : MonoBehaviour {
    private bool canShoot = true;
    public GameObject waterParticle;
    public Collider player;
    public LineRenderer line;
    public List<GameObject> waterDrops = new List<GameObject>();
    public Material waterMaterial;
    private float hoseRate = 50f;
    private float nextTimeToHose = 0f;

	void Start () {
        line = gameObject.AddComponent<LineRenderer>();
        line.material = waterMaterial;
        line.startWidth = .6f;
        line.endWidth = .6f;
    }
    void Update () {
        Vector3 direction = /*transform.forward + */transform.position;
        if (Input.GetKey("space") && Time.time >= nextTimeToHose && canShoot)
        {
            nextTimeToHose = Time.time + 1f / hoseRate;
            ShootWater(direction);
        }
        if (Input.GetKeyUp("space"))
        {
            foreach(GameObject waterParticle in waterDrops)
            {
                Destroy(waterParticle, 10f);
            }
            canShoot = false;
        }
        makeStream();
        if(!canShoot && waterDrops.Count == 0)
        {
            Destroy(gameObject);
        }
    }
    void ShootWater(Vector3 direction)
    {
        GameObject waterDrop = Instantiate(waterParticle, direction, transform.rotation);
        waterDrop.GetComponent<Projectile>().spawnOrigin = gameObject;
        //waterDrop.GetComponent<Projectile>().player = player;
        startMovement(waterDrop);
        waterDrops.Add(waterDrop);
    }
    void startMovement(GameObject waterDrop)
    {
        waterDrop.GetComponent<Rigidbody>().velocity = waterDrop.transform.forward * 5f;
    }
    void makeStream()
    {
        line.positionCount = waterDrops.Count;
        var points = new Vector3[line.positionCount];
        for (int i = 0; i < waterDrops.Count; i++)
        {
            points[i] = waterDrops[i].transform.position;
        }
        line.SetPositions(points);
    }
}
