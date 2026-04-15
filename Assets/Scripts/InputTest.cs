using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public InputActionProperty ActionButton;

    private GameObject touchingObject;
    private GameObject heldObject;

    [SerializeField] private Rigidbody HandRB;

    void Update()
    {
        bool button = ActionButton.action.IsPressed();

        if (button && touchingObject)
        {
            GrabObject();
            Debug.Log("Grabbing object");
        }

        if (!button && heldObject)
        {
            ReleaseObject();
            Debug.Log("releasing object");
        }
    }

    void GrabObject()
    {
        heldObject = touchingObject;
        if (GetComponent<FixedJoint>() == null)
        {
            var joint = AddFixedJoint();
            joint.connectedBody = heldObject.GetComponent<Rigidbody>();
        }
    }

    void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            heldObject.GetComponent<Rigidbody>().linearVelocity = HandRB.linearVelocity;
            heldObject.GetComponent<Rigidbody>().angularVelocity = HandRB.angularVelocity;
        }
        heldObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        touchingObject = other.gameObject;
        Debug.Log("Got Object");
    }

    void OnTriggerExit(Collider other)
    {
        touchingObject = null;
        Debug.Log("Left Object");
    }

    FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.massScale = 0f;
        return fx;
    }
}
