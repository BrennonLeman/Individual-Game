using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private RectTransform scoreTick;
    
    private Vector3 startingPosition = new Vector3(-347.3f, -195.36f, 0f);
    
    private Vector3 endPosition = new Vector3(-347.3f, 195.36f, 0f);

    public Utter activeUtter;

    private float time = 2f;

    [SerializeField] private AnimationCurve curve;
    
    [SerializeField] private List<Sprite> activeBars = new List<Sprite>();
    private List<Sprite> bufferBars = new List<Sprite>();
    private List<Sprite> usedBars = new List<Sprite>();

    [SerializeField] private Image scoreBackground;
    
    public enum State {
        Idle,
        Counting
    }
    public State state;
    void Start()
    {
        instance = this;
        NextState();
    }

    private void Awake()
    {
        activeUtter = null;
    }

    void Update()
    {
        
    }
    
    IEnumerator IdleState () {
        Debug.Log("Idle: Enter");
        scoreTick.localPosition = startingPosition;
        while (state == State.Idle)
        {
            yield return 0;
        }
        Debug.Log("Idle: Exit");
        NextState();
    }
    IEnumerator CountingState () {
        Debug.Log("Counting: Enter");
//        if (activeBars.Count == 0)
//        {
//            for (var i = 0; i < usedBars.Count; i++)
//            {
//                activeBars.Add(usedBars[i]);
//            }
//            usedBars.Clear();
//            
//        }
//        var temp = activeBars[Random.Range(0, activeBars.Count)];
//        activeBars.Remove(temp);
//        bufferBars.Add(temp);
//        if (bufferBars.Count > 1)
//        {
//            usedBars.Add(bufferBars[0]);
//            bufferBars.Remove(bufferBars[0]);
//        }
//
//        
//        scoreBackground.sprite = temp;
//        
//        
//        float journey = 0f;
//        scoreTick.position = startingPosition;
        while (state == State.Counting)
        {
//            if (journey <= time)
//            {
//                journey += Time.deltaTime;
//                float percent = Mathf.Clamp01(journey / time);
//                float curvePercent = curve.Evaluate(percent);
//                scoreTick.localPosition = Vector3.Lerp(startingPosition, endPosition, curvePercent);
//            }
            yield return 0;
        }
        Debug.Log("Counting: Exit");
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
}
