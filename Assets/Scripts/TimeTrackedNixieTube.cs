using System.Collections.Generic;
using UnityEngine;
public class TimeTrakedNixeTube : TimeTrakedObject
{
    //Lists
    List<int> trackedNumber;



    //Special Behavoiur

    NixieTube nixieTube;

    public override void Init()
    {
        trackedNumber = new();
        nixieTube = GetComponent<NixieTube>();
        if (nixieTube == null)
        {
            Debug.LogError("No NixieTube Component");
        }
    }
    public override bool IsSameAsLastFrame(int index)
    {
        return trackedNumber[index] == nixieTube.numb;
    }

    public override void UpdateGameObject(int index)
    {
        nixieTube.numb = trackedNumber[index];
        nixieTube.SetText();
    }
    public override void AddToLists()
    {
        trackedNumber.Add(nixieTube.numb);
    }
    public override void RemoveOnList(int index)
    {
        trackedNumber.RemoveAt(index);
    }

    public override void StartRewind()
    {
        base.StartRewind();
        nixieTube.Disabled = true;
    }
    public override void StopRewind()
    {
        base.StopRewind();
        nixieTube.Disabled = false;
    }
}
