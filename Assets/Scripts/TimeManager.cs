using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static TimeManager _instance;

    //Delegates
    public delegate void Test();
    static public Test test;
    [SerializeField] int maxFrames;
    static public int MaxFrames;

    [SerializeField] bool DebugButton;
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
        MaxFrames = maxFrames;
    }

    // Update is called once per frame
    void Update()
    {
        if (DebugButton)
        {
            DebugButton = false;
            test();
        }
    }

    void FixedUpdate()
    {

    }
}
