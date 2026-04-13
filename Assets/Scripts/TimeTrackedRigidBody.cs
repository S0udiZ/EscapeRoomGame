using System.Collections.Generic;
using UnityEngine;
public class TimeTrakedRigidBody : TimeTrakedObject
{
    //Lists
    List<Vector3> trackedPostion;
    List<Quaternion> trackedRotation;
    List<Vector3> trackedVelocity;
    List<Vector3> trackedAngularVelocity;



    //Special Behavoiur
    Vector3 rewindVelocity;
    Vector3 rewindAngularVelocity;
    bool updateRewindVelocity;
    Spring spring;

    bool isSpring = false;

    Rigidbody rb;

    public override void Init()
    {
        spring = GetComponent<Spring>();
        if (spring != null)
        {
            isSpring = true;
        }
        trackedPostion = new();
        trackedRotation = new();
        trackedVelocity = new();
        trackedAngularVelocity = new();
        rb = GetComponent<Rigidbody>();
    }
    public override bool IsSameAsLastFrame(int index)
    {
        return trackedPostion[index] == transform.position && trackedRotation[index] == transform.rotation && trackedVelocity[index] == rb.linearVelocity;
    }

    public override void UpdateGameObject(int index)
    {
        transform.position = trackedPostion[index];
        transform.rotation = trackedRotation[index];
        rewindVelocity = trackedVelocity[index];
        rewindAngularVelocity = trackedAngularVelocity[index];
    }
    public override void AddToLists()
    {
        trackedPostion.Add(transform.position);
        trackedRotation.Add(transform.rotation);
        trackedVelocity.Add(rb.linearVelocity);
        trackedAngularVelocity.Add(rb.angularVelocity);
    }
    public override void RemoveOnList(int index)
    {
        trackedPostion.RemoveAt(index);
        trackedRotation.RemoveAt(index);
        trackedVelocity.RemoveAt(index);
        trackedAngularVelocity.RemoveAt(index);
    }

    public override void StartRewind()
    {
        base.StartRewind();
        rb.isKinematic = true;
        if (isSpring)
        {
            spring.enabled = false;
        }
    }
    public override void StopRewind()
    {
        base.StopRewind();
        rb.isKinematic = false;
        if (isSpring)
        {
            spring.enabled = true;
        }
    }

    //Extra Behavoiour
    public override void ExtraBehaivourFixedUpdate()
    {
        if (updateRewindVelocity && !rewind)
        {
            rb.linearVelocity = rewindVelocity;
            rb.angularVelocity = rewindAngularVelocity;
            updateRewindVelocity = false;
        }
    }

    public override void ExtraBehaivourTimeRewind()
    {
        updateRewindVelocity = true;
    }
}
