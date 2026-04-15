using System.Collections.Generic;
using System.Linq;
using SaintsField;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;
    [SerializeField] float ActivationTime;
    [SerializeField] float MinActivationLength;
    // [SerializeField] Interactable[] ActivationTargets;
    [SerializeField] private SaintsDictionary<Interactable[], PresurePlate[]> ActivationTargets;
    float curActivationTime;
    float curTimeActivated;
    bool activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == TargetObject)
        {
            curActivationTime = 0;
            print("Button Pressed");
        }

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == TargetObject && !activated)
        {
            print($"Button Pressed {gameObject.name}");
            curActivationTime += Time.deltaTime;
            if (curActivationTime > ActivationTime)
            {
                activated = true;
                SendActivation();
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == TargetObject && activated)
        {
            activated = false;
            SendDeActivation();
        }
    }

    void FixedUpdate()
    {

    }

    // void SendActivation()
    // {
    //     if (ActivationTargets.Length == 0) { return; }
    //     foreach (Interactable ActivationTarget in ActivationTargets)
    //     {
    //         ActivationTarget.Activate();
    //     }
    // }
    // void SendDeActivation()
    // {
    //     if (ActivationTargets.Length == 0) { return; }
    //     foreach (Interactable ActivationTarget in ActivationTargets)
    //     {
    //         ActivationTarget.DeActivate();
    //     }
    // }

    void SendActivation()
    {
        if (ActivationTargets.Count == 0) { return; }
        foreach (KeyValuePair<Interactable[], PresurePlate[]> ActivationTarget in ActivationTargets)
        {
            if (ActivationTarget.Value.Length == 0)
            {
                foreach (var interactable in ActivationTarget.Key)
                {
                    interactable.Activate();
                }
            }
            else if (ActivationTarget.Value.All(plate => plate.activated))
            {
                foreach (var interactable in ActivationTarget.Key)
                {
                    interactable.Activate();
                }
            }
        }
    }
    void SendDeActivation()
    {
        if (ActivationTargets.Count == 0) { return; }
        foreach (KeyValuePair<Interactable[], PresurePlate[]> ActivationTarget in ActivationTargets)
        {
            foreach (var interactable in ActivationTarget.Key)
            {
                interactable.DeActivate();
            }
        }
    }
}
