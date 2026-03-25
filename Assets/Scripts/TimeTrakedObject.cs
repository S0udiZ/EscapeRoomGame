using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeTrakedObject : MonoBehaviour
{
    List<Vector3> trackedPostion;
    List<Quaternion> trackedRotation;
    List<Vector3> trackedVelocity;
    List<Vector3> trackedAngularVelocity;
    List<int> count;
    int listIndex = -1;
    int currentCount = 0;
    [SerializeField] bool rewind;
    Vector3 rewindVelocity;
    Vector3 rewindAngularVelocity;
    bool updateRewindVelocity;


    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trackedPostion = new();
        trackedRotation = new();
        trackedVelocity = new();
        trackedAngularVelocity = new();
        count = new();
    }
    void Awake()
    {
        TimeManager.RewindStart += StartRewind;
        TimeManager.RewindStop += StopRewind;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rewind)
        {
            rb.isKinematic = true;

            TimeRewind();
        }
        else
        {
            rb.isKinematic = false;
            if (updateRewindVelocity)
            {
                rb.linearVelocity = rewindVelocity;
                rb.angularVelocity = rewindAngularVelocity;
                updateRewindVelocity = false;
            }
            TimeLog();
        }


        //DebugCount();
    }

    void TimeLog()
    {
        if (listIndex != -1 && trackedPostion[listIndex] == transform.position && trackedRotation[listIndex] == transform.rotation && trackedVelocity[listIndex] == rb.linearVelocity)
        {
            //print("Same Object Skiping");
            count[listIndex]++;
        }
        else
        {
            //print("Updating Object");

            AddToTrackedList();
        }
        currentCount++;

        if (currentCount > TimeManager.MaxFrames)
        {
            RemoveIndexOnLists(0);
        }
    }

    void TimeRewind()
    {
        //Guard Clause
        if (currentCount < 1)
        {
            Debug.Log("Ran out of Rewind Space");
            return;
        }

        transform.position = trackedPostion[listIndex];
        transform.rotation = trackedRotation[listIndex];
        rewindVelocity = trackedVelocity[listIndex];
        rewindAngularVelocity = trackedAngularVelocity[listIndex];
        updateRewindVelocity = true;
        RemoveIndexOnLists(listIndex);


    }

    void AddToTrackedList()
    {
        trackedPostion.Add(transform.position);
        trackedRotation.Add(transform.rotation);
        trackedVelocity.Add(rb.linearVelocity);
        trackedAngularVelocity.Add(rb.angularVelocity);
        count.Add(1);
        listIndex++;
    }
    void RemoveIndexOnLists(int index)
    {
        if (count[index] > 1)
        {
            count[index]--;
        }
        else
        {
            trackedPostion.RemoveAt(index);
            trackedRotation.RemoveAt(index);
            trackedVelocity.RemoveAt(index);
            trackedAngularVelocity.RemoveAt(index);
            count.RemoveAt(index);
            listIndex--;

        }
        currentCount--;


    }

    void StartRewind()
    {
        rewind = true;
    }

    void StopRewind()
    {
        rewind = false;
    }

    void DebugCount()
    {
        int allcount = 0;
        foreach (int countamount in count)
        {
            allcount += countamount;
        }
        print(allcount - currentCount);
    }
}
