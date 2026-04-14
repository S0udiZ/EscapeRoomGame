using System;
using System.Collections;
using Meta.Net.NativeWebSocket;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;

    //Delegates
    public delegate void RewindFunc();
    static public RewindFunc RewindStart;
    static public RewindFunc RewindStop;
    [SerializeField] int maxFrames = 600;
    [SerializeField] float slowTime = 1;
    static public int MaxFrames;

    [SerializeField] bool DebugButtonRStart;
    [SerializeField] bool DebugButtonRStop;
    float StartFixedDeltaTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Singleton Behaivour
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        StartFixedDeltaTime = Time.fixedDeltaTime;
        MaxFrames = maxFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugButtonRStart)
        {
            DebugButtonRStart = false;
            StartCoroutine(StartRewind());
        }
        if (DebugButtonRStop)
        {
            DebugButtonRStop = false;
            RewindStop();
        }
    }

    void FixedUpdate()
    {

    }

    /// <summary>
    /// StartRewind is a corutine that lerps between 2 time scales to create a 
    /// smooth transtion into reverse time
    /// </summary>
    IEnumerator StartRewind()
    {
        float currentSlowTime = 0;
        while (currentSlowTime < slowTime)
        {
            currentSlowTime += Time.deltaTime * (1 / Time.timeScale);
            Time.timeScale = Mathf.Lerp(1f, 0.1f, currentSlowTime / slowTime);
            yield return null;
        }
        RewindStart();
        currentSlowTime = 0;
        while (currentSlowTime < slowTime)
        {
            currentSlowTime += Time.deltaTime * (1 / Time.timeScale);
            Time.timeScale = Mathf.Lerp(0.1f, 1f, currentSlowTime / slowTime);
            yield return null;
        }
        Time.timeScale = 1;
    }

    public void StartRewindAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCoroutine(StartRewind());
        }
    }

    public void StopRewindAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RewindStop();
        }
    }
}
