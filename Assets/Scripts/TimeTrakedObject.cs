using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeTrakedObject : MonoBehaviour
{
    List<Vector3> trackedPostion;
    List<Quaternion> trackedRotation;
    List<Vector3> trackedVelocity;
    List<int> count;
    int index = 0;


    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trackedPostion = new();
        trackedRotation = new();
        trackedVelocity = new();
        count = new();
        trackedPostion.Add(transform.position);
        trackedRotation.Add(transform.rotation);
        trackedVelocity.Add(rb.linearVelocity);
        count.Add(1);
    }
    void Awake()
    {
        TimeManager.test += TestObj;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (trackedPostion[index] == transform.position && trackedRotation[index] == transform.rotation && trackedVelocity[index] == rb.linearVelocity)
        {
            //print("Same Object Skiping");
            count[index]++;
        }
        else
        {
            //print("Updating Object");
            if (trackedPostion.Count > TimeManager.MaxFrames)
            {
                trackedPostion.RemoveAt(0);
                trackedRotation.RemoveAt(0);
                trackedVelocity.RemoveAt(0);
            }
            trackedPostion.Add(transform.position);
            trackedRotation.Add(transform.rotation);
            trackedVelocity.Add(rb.linearVelocity);
            count.Add(1);
        }
    }

    void TestObj()
    {

    }
}
