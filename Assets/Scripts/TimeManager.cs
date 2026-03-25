using System;
using System.Collections;
using Meta.Net.NativeWebSocket;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;

    //Delegates
    public delegate void RewindFunc();
    public delegate void SetBool(bool Value);
    static public RewindFunc RewindStart;
    static public RewindFunc RewindStop;
    [SerializeField] int maxFrames;
    [SerializeField] float slowTime;
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
        //Debug.Log($"{Time.timeScale}");
    }

    void FixedUpdate()
    {

    }
    IEnumerator StartRewind()
    {
        float currentSlowTime = 0;
        while (currentSlowTime < slowTime)
        {
            currentSlowTime += Time.deltaTime * (1 / Time.timeScale);
            Time.timeScale = Mathf.Lerp(1f, 0.1f, currentSlowTime / slowTime);
            //Time.fixedDeltaTime = StartFixedDeltaTime * Time.timeScale;
            yield return null;
        }
        RewindStart();
        currentSlowTime = 0;
        while (currentSlowTime < slowTime)
        {
            currentSlowTime += Time.deltaTime * (1 / Time.timeScale);
            Time.timeScale = Mathf.Lerp(0.1f, 1f, currentSlowTime / slowTime);
            //Time.fixedDeltaTime = StartFixedDeltaTime * Time.timeScale;
            yield return null;
        }
        Time.timeScale = 1;
        //Time.fixedDeltaTime = StartFixedDeltaTime * Time.timeScale;
    }
}
