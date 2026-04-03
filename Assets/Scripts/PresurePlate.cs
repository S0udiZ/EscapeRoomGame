using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] GameObject TargetObject;
    [SerializeField] float ActivationTime;
    [SerializeField] Interactable[] ActivationTargets;
    float curActivationTime;
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
            print("Button Pressed");
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

    void SendActivation()
    {
        if (ActivationTargets.Length == 0) { return; }
        foreach (Interactable ActivationTarget in ActivationTargets)
        {
            ActivationTarget.Activate();
        }
    }
    void SendDeActivation()
    {
        if (ActivationTargets.Length == 0) { return; }
        foreach (Interactable ActivationTarget in ActivationTargets)
        {
            ActivationTarget.DeActivate();
        }
    }
}
