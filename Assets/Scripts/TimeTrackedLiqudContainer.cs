using UnityEngine;
using System.Collections.Generic;
using UnitySimpleLiquid;

public class TimeTrackedLiqudContainer : TimeTrakedObject
{
    //Lists
    List<Vector3> trackedPostion;
    List<Quaternion> trackedRotation;
    List<Vector3> trackedVelocity;
    List<Vector3> trackedAngularVelocity;



    //Special Behavoiur

    LiquidContainer liquidContainer;


    public override void Init()
    {
        liquidContainer = GetComponent<LiquidContainer>();
        if (!liquidContainer)
        {
            Debug.LogError("Missing LiquidContainer for TimeTrackedLiqudContainer");
        }
    }
    public override bool IsSameAsLastFrame(int index)
    {
        return true;
    }

    public override void UpdateGameObject(int index)
    {

    }
    public override void AddToLists()
    {

    }
    public override void RemoveOnList(int index)
    {

    }

    public override void StartRewind()
    {
        base.StartRewind();

    }
    public override void StopRewind()
    {
        base.StopRewind();

    }

    //Extra Behavoiour
    public override void ExtraBehaivourFixedUpdate()
    {

    }

    public override void ExtraBehaivourTimeRewind()
    {

    }
}
