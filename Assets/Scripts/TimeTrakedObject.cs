using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public abstract class TimeTrakedObject : MonoBehaviour
{


    List<int> count;
    int listIndex = -1;
    int currentCount = 0;
    [SerializeField] protected bool rewind;
    public bool TimeExempt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Init();
        count = new();
    }

    /// <summary>
    /// Initialize Lists
    /// </summary>
    public abstract void Init();
    void Awake()
    {
        TimeManager.RewindStart += StartRewind;
        TimeManager.RewindStop += StopRewind;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ExtraBehaivourFixedUpdate();

        if (rewind && !TimeExempt)
        {
            TimeRewind();
        }
        else
        {

            TimeLog();
        }


        //DebugCount();
    }

    /// <summary>
    /// Runs at the start of Fixed update to allow for special behavoiur
    /// </summary>
    virtual public void ExtraBehaivourFixedUpdate() { }

    /// <summary>
    /// boolean expresion to check if none of the tracked variabels has changed.
    /// if it is the same it should return true if not return false
    /// </summary>
    /// <param name="index">the index of the lists that the values should be comapred to</param>
    /// <returns>bool</returns>
    public abstract bool IsSameAsLastFrame(int index);
    void TimeLog()
    {
        if (listIndex != -1 && IsSameAsLastFrame(listIndex))
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
            return;
        }


        ExtraBehaivourTimeRewind();

        UpdateGameObject(listIndex);

        RemoveIndexOnLists(listIndex);
    }

    /// <summary>
    /// Runs at the start of TimeRewind to allow for special behavoiur
    /// </summary>
    public virtual void ExtraBehaivourTimeRewind() { }


    /// <summary>
    /// Here you read from the lists at index and put it back into the GameObject
    /// </summary>
    public abstract void UpdateGameObject(int index);

    public void AddToTrackedList()
    {
        AddToLists();
        count.Add(1);
        listIndex++;
    }

    /// <summary>
    /// Add all varibles to the tracked lists here
    /// </summary>
    public abstract void AddToLists();

    void RemoveIndexOnLists(int index)
    {
        if (count[index] > 1)
        {
            count[index]--;
        }
        else
        {
            RemoveOnList(index);
            count.RemoveAt(index);
            listIndex--;

        }
        currentCount--;
    }

    /// <summary>
    /// Remove item from tracked lists at index
    /// </summary>
    /// <param name="index"></param>
    public abstract void RemoveOnList(int index);

    public virtual void StartRewind()
    {
        rewind = true;

    }

    public virtual void StopRewind()
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
