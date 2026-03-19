using UnityEngine;

public class TimeTrakedObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void Awake()
    {
        TimeManager.test += TestObj;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TestObj()
    {
        print(gameObject.name);
    }
}
