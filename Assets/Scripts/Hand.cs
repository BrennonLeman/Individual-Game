using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Transform utter_1;
    [SerializeField] private Transform utter_2;
    [SerializeField] private Transform utter_3;
    [SerializeField] private Transform utter_4;

    [SerializeField] private Transform elbow;
    [SerializeField] private Transform shoulder;

    [SerializeField] private Material armMat;

    private LineRenderer arm;

    private bool frontLeft = false;

    private bool frontRight = false;

    private bool backLeft = false;

    private bool backRight = false;

    private bool inIdlePos = true;

    [HideInInspector] public bool squeeze = false;

    private Vector3 idlePos;
    
    [SerializeField] private Transform hand;

    private float lerpFactor = 20f;

    private enum ActiveDirection
    {
        frontLeft,
        frontRight,
        backLeft,
        backRight,
        none
    }

    private ActiveDirection dir;
    public enum State {
        Idle,
        GrabbingFrontLeft,
        GrabbingFrontRight,
        GrabbingBackLeft,
        GrabbingBackRight
    }
    public State state;
    
    private void Start()
    {
        arm = gameObject.AddComponent<LineRenderer>();
        idlePos = transform.position;
        dir = ActiveDirection.none;
        arm.startWidth = 0.7f;
        arm.endWidth = 0.7f;
        arm.material = armMat;
        arm.numCapVertices = 16;
        arm.SetPositions(new Vector3[]
        {
            transform.position, transform.position
        });
        NextState();
    }
    
    IEnumerator IdleState () {
        Debug.Log("Idle: Enter");
        if (Vector3.Distance(hand.position, idlePos) <= 0.1f)
        {
            inIdlePos = true;
        }
        else inIdlePos = false;
        
        while (state == State.Idle)
        {
            if (!inIdlePos)
            {
                if (Vector3.Distance(hand.position, idlePos) >= 0.1f)
                {
                    hand.position = Vector3.Lerp(hand.position, idlePos, Time.deltaTime * lerpFactor);
                }
                else inIdlePos = true;
            }
            if (dir == ActiveDirection.frontLeft) state = State.GrabbingFrontLeft;
            else if (dir == ActiveDirection.frontRight) state = State.GrabbingFrontRight;
            else if (dir == ActiveDirection.backLeft) state = State.GrabbingBackLeft;
            else if (dir == ActiveDirection.backRight) state = State.GrabbingBackRight;
            yield return 0;
        }
        Debug.Log("Idle: Exit");
        NextState();
    }
    
    IEnumerator GrabbingFrontLeftState () {
        
        Debug.Log("GrabbingFrontLeft: Enter");
        var activeUtter = utter_2.parent.gameObject.GetComponent<Utter>();
        activeUtter.isGrabbed = true;
        while (state == State.GrabbingFrontLeft)
        {
            if (Vector3.Distance(hand.position, utter_2.position) >= 0.1f)
            {
                hand.position = Vector3.Lerp(hand.position, utter_2.position + hand.up * 0.8f, Time.deltaTime * lerpFactor);
            }
            if (dir == ActiveDirection.frontRight) state = State.GrabbingFrontRight;
            else if (dir == ActiveDirection.backLeft) state = State.GrabbingBackLeft;
            else if (dir == ActiveDirection.backRight) state = State.GrabbingBackRight;
            else if (dir == ActiveDirection.none) state = State.Idle;
            yield return 0;
        }

        GameManager.instance.state = GameManager.State.Idle;
        activeUtter.isGrabbed = false;
        Debug.Log("GrabbingFrontLeft: Exit");
        NextState();
    }
    
    IEnumerator GrabbingFrontRightState () {
        Debug.Log("GrabbingFrontRight: Enter");
        var activeUtter = utter_1.parent.gameObject.GetComponent<Utter>();
        activeUtter.isGrabbed = true;
        while (state == State.GrabbingFrontRight)
        {
            if (Vector3.Distance(hand.position, utter_1.position) >= 0.1f)
            {
                hand.position = Vector3.Lerp(hand.position, utter_1.position + hand.up * 0.8f, Time.deltaTime * lerpFactor);
            }
            if (dir == ActiveDirection.frontLeft) state = State.GrabbingFrontLeft;
            else if (dir == ActiveDirection.backLeft) state = State.GrabbingBackLeft;
            else if (dir == ActiveDirection.backRight) state = State.GrabbingBackRight;
            else if (dir == ActiveDirection.none) state = State.Idle;
            yield return 0;
        }
        GameManager.instance.state = GameManager.State.Idle;
        activeUtter.isGrabbed = false;
        Debug.Log("GrabbingFrontRight: Exit");
        NextState();
    }
    
    IEnumerator GrabbingBackLeftState () {
        Debug.Log("GrabbingBackLeft: Enter");
        var activeUtter = utter_3.parent.gameObject.GetComponent<Utter>();
        activeUtter.isGrabbed = true;
        while (state == State.GrabbingBackLeft)
        {
            if (Vector3.Distance(hand.position, utter_3.position) >= 0.1f)
            {
                hand.position = Vector3.Lerp(hand.position, utter_3.position + hand.up * 0.8f, Time.deltaTime * lerpFactor);
            }
            if (dir == ActiveDirection.frontRight) state = State.GrabbingFrontRight;
            else if (dir == ActiveDirection.frontLeft) state = State.GrabbingFrontLeft;
            else if (dir == ActiveDirection.backRight) state = State.GrabbingBackRight;
            else if (dir == ActiveDirection.none) state = State.Idle;
            yield return 0;
        }
        GameManager.instance.state = GameManager.State.Idle;
        activeUtter.isGrabbed = false;
        Debug.Log("GrabbingBackLeft: Exit");
        NextState();
    }
    
    IEnumerator GrabbingBackRightState () {
        Debug.Log("GrabbingBackRight: Enter");
        var activeUtter = utter_4.parent.gameObject.GetComponent<Utter>();
        activeUtter.isGrabbed = true;
        while (state == State.GrabbingBackRight)
        {
            if (Vector3.Distance(hand.position, utter_4.position) >= 0.1f)
            {
                hand.position = Vector3.Lerp(hand.position, utter_4.position + hand.up * 0.8f, Time.deltaTime * lerpFactor);
            }
            if (dir == ActiveDirection.frontRight) state = State.GrabbingFrontRight;
            else if (dir == ActiveDirection.frontLeft) state = State.GrabbingFrontLeft;
            else if (dir == ActiveDirection.backLeft) state = State.GrabbingBackLeft;
            else if (dir == ActiveDirection.none) state = State.Idle;
            yield return 0;
        }
        GameManager.instance.state = GameManager.State.Idle;
        activeUtter.isGrabbed = false;
        Debug.Log("GrabbingBackRight: Exit");
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

    // Update is called once per frame
    void Update()
    {
        drawArm();
        updateInputs();
        if (frontLeft) dir = ActiveDirection.frontLeft;
        else if (frontRight) dir = ActiveDirection.frontRight;
        else if (backLeft) dir = ActiveDirection.backLeft;
        else if (backRight) dir = ActiveDirection.backRight;
        else dir = ActiveDirection.none;
    }

    private void drawArm()
    {
        BrennonsFunctions.LineRendererBezier(arm, hand.position, elbow.position, shoulder.position, 30);
    }
    private void updateInputs()
    {
        frontLeft = (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) ? true : false;
        frontRight = (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) ? true : false;
        backLeft = (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) ? true : false;
        backRight = (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) ? true : false;

        squeeze = (Input.GetKey(KeyCode.Space));
    }
}
