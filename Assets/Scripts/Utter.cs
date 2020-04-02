using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using Random = System.Random;

public class Utter : MonoBehaviour
{
    [SerializeField] private Transform endPoint;
    
    private Vector3 endStartingPoint = Vector3.zero;

    private Vector3 endMilkingPoint;
    
    [HideInInspector] public Vector3 midPoint;
    
    private LineRenderer ln;
    
    [SerializeField] private Material _materialFront;
    
    [SerializeField] private Material _materialBack;

    private float lerpFactor = 20f;
    
    public bool isGrabbed = false;

    [SerializeField] private Hand hand;

    private float utterWidth = 0.9f;

    private float time = 1f;
    private float changeBackTime = 0.2f;

    [SerializeField] private AnimationCurve curve;

    [SerializeField] private AnimationCurve utterColorCurve;
    public enum State {
        Idle,
        Grabbed
    }
    public State state;
    [SerializeField] private bool isBackUtter = false;

    [HideInInspector] public bool canShoot = true;
    
    private int randRange;

    private Color startingFront;
    private Color startingBack;
    [SerializeField] private Color finalColorBack;
    [SerializeField] private Color finalColorFront;
    private void Start()
    {
        NextState();
    }

    private void Awake()
    {
        startingFront = new Color(1f, .7058f, .8509f);
        startingBack = new Color(0.8235f, 0.5803f, 0.7019f);
        //finalColorBack = Color.red;
        randRange = UnityEngine.Random.Range(1, 4);
        endStartingPoint = endPoint.position;
        endMilkingPoint = endStartingPoint - new Vector3(0f, 2f, 0f);
        ln = gameObject.AddComponent<LineRenderer>();
        ln.positionCount = 2;
        ln.numCapVertices = 16;
        ln.startWidth = utterWidth;
        ln.endWidth = utterWidth;
        ln.receiveShadows = false;
        ln.material = isBackUtter ? _materialBack : _materialFront;
        ln.SetPositions(new Vector3[]
        {
            transform.position, endPoint.transform.position
        });
        midPoint = (transform.position + endPoint.position) / 2f;
        //ln.material.color = isBackUtter ? startingBack : startingFront;
    }
    
    IEnumerator IdleState () {
        Debug.Log("Idle: Enter");
        
        randRange = UnityEngine.Random.Range(1, 4);
        
        canShoot = false;
        
        //ln.material.color = isBackUtter ? startingBack : startingFront;
        var c = ln.material.color;
        var journey = 0f;
        while (state == State.Idle)
        {
            if (!c.Equals(isBackUtter ? startingBack : startingFront))
            {
                if (journey <= changeBackTime)
                {
                    journey += Time.deltaTime;
                    float percent = BrennonsFunctions.scale(0f, 0.2f, 0f, 1f, journey / time);
                    ln.material.color = Color.Lerp(c, isBackUtter ? startingBack : startingFront, percent);
                }
                
            }
            endPoint.position = endStartingPoint + new Vector3(Mathf.Sin(randRange * Time.time) * .4f, 0f, Mathf.Cos(randRange + Time.time) * .4f);
            if (isGrabbed) state = State.Grabbed;
            yield return 0;
        }
        Debug.Log("Idle: Exit");
        NextState();
    }
    IEnumerator GrabbedState () {
        Debug.Log("Grabbed: Enter");
        
        GameManager.instance.state = GameManager.State.Counting;
        GameManager.instance.activeUtter = this;
        var shooter = GetComponentInChildren<MilkShooter>();
        
        canShoot = true;
        shooter.shootStreamInstance();
        //var stream = GetComponentInChildren<MilkStream>();
        //stream.streamWidth = 0.2f;
        float journey = 0f;
        while (state == State.Grabbed)
        {
            if (journey <= time)
            {
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey / time);
                float curvePercent = curve.Evaluate(percent);
                float utterColorPercent = utterColorCurve.Evaluate(percent);
                ln.material.color = isBackUtter ? Color.Lerp(startingBack, finalColorBack, utterColorPercent) : Color.Lerp(startingFront, finalColorFront, utterColorPercent);
                endPoint.position = Vector3.Lerp(endStartingPoint, endMilkingPoint, curvePercent);
                
            }
            else canShoot = false;

            if (!isGrabbed) state = State.Idle;
            yield return 0;
        }
        Debug.Log("Grabbed: Exit");
        NextState();
    }

    private void NextState () {
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }
    
    void Update()
    {
        BrennonsFunctions.LineRendererBezier(ln, transform.position, midPoint, endPoint.position, 20);
    }
}
