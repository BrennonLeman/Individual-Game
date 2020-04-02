using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkShooter : MonoBehaviour
{
    [SerializeField] private GameObject milkStream;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //shootStreamInstance();
        }
    }

    public void shootStreamInstance()
    {
        GameObject mS = Instantiate(milkStream, transform.position, Quaternion.identity);
        mS.transform.parent = transform;

    }
}
