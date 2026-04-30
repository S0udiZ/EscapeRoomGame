using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour
{

    [Tooltip("The head variables")]
    public Head head;
    [Tooltip("The controller variables")]
    public Controller l_hand, r_hand;
    public LayerMask teleport_colliders;

    private void Start()
    {
        head.body = gameObject;
        l_hand.body = gameObject;
        l_hand.left_controller = true;
        r_hand.body = gameObject;
        head.Setup();
        l_hand.Setup();
        r_hand.Setup();
        r_hand.controller_thumbstick_pressed.performed += TeleportStart;
        r_hand.controller_thumbstick_pressed.canceled += TeleportEnd;
    }

    public void TeleportStart(InputAction.CallbackContext context)
    {
        teleporting = true;
        StartCoroutine(TeleportLine());
    }
    public void TeleportEnd(InputAction.CallbackContext context)
    {

        //TODO: make physics based movement so people cant glitch hands threw walls
        teleporting = false;
        RaycastHit hit;
        if (Physics.Raycast(r_hand.controller.transform.position, r_hand.controller.transform.TransformDirection(Vector3.forward), out hit, 5 * transform.localScale.y, teleport_colliders))
        {
            Vector3 teleport_pos = VR_Teleport.Teleport(hit.point);
            //Debug.Log(teleport_pos);
            if (teleport_pos != new Vector3(9999, 9999, 9999))
            {
                transform.position = teleport_pos;
            }
        }
    }
    bool teleporting;
    IEnumerator TeleportLine()
    {
        LineRenderer line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 2;
        RaycastHit hit;
        while (teleporting)
        {
            if(Physics.Raycast(r_hand.controller.transform.position, r_hand.controller.transform.forward, out hit, 5 * transform.localScale.y, teleport_colliders))
            {
                line.startColor = Color.blue;
                line.endColor = Color.red;
                if (VR_Teleport.Teleport(hit.point) != new Vector3(9999,9999,9999))
                {
                    line.endColor = Color.green;
                }
                line.SetPosition(1, hit.point);
            }
            else
            {
                line.startColor = Color.clear;
                line.endColor = Color.clear;
                line.SetPosition(1, Vector3.zero);
            }
            line.SetPosition(0, r_hand.controller.transform.position);
            //Debug.Log(hit.transform);
            yield return null;
        }
        line.positionCount = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(l_hand.controller.transform.position, l_hand.controller.transform.position + l_hand.controller.transform.right * 0.1f);
        Gizmos.DrawLine(r_hand.controller.transform.position, r_hand.controller.transform.position + r_hand.controller.transform.right * -0.1f);
    }
}

[System.Serializable]
public class Head
{
    [Tooltip("the head object\n(has to lay under the player object)")]
    public GameObject head;
    [Tooltip("Used for mouth sync and eye sync if enabled")]
    public Animator animator;
    [Tooltip("head position"), Header("Head position")]
    public InputAction
        hmd_position_action;
    [Tooltip("head rotation input"), Header("Head rotation")]
    public InputAction
        hmd_rotation_action;
    [HideInInspector]    
    public GameObject body;
    Rigidbody rb;

    public void Setup()
    {
        hmd_position_action.Enable();
        hmd_rotation_action.Enable();
        hmd_position_action.performed += UpdateHMDPos;
        hmd_rotation_action.performed += UpdateHMDRot;
        rb = head.GetComponent<Rigidbody>();
    }

    void UpdateHMDPos(InputAction.CallbackContext context)
    {
        //TODO: make head movement physics based so people cant glitch threw walls
        head.transform.position = Vector3.Scale(context.ReadValue<Vector3>(), body.transform.localScale) + body.transform.position;
    }

    void UpdateHMDRot(InputAction.CallbackContext context)
    {
        head.transform.rotation = context.ReadValue<Quaternion>();
    }
}

[System.Serializable]
public class Controller
{

    [Tooltip("the controller/hand object\n(has to lay under the player object)")]
    public GameObject controller;
    [Tooltip("the hand object. used to offset the hands from the controllers")]
    GameObject hand;
    [Tooltip("Used for fingers sync and object grabbing")]
    public Animator controller_animator;
    [HideInInspector]
    public bool left_controller;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public GameObject body;
    [Tooltip("controller position input"), Header("Controller position")]
    public InputAction
        controlle_position;
    [Tooltip("controller rotation input"), Header("Controller rotation")]
    public InputAction
        controller_rotation;
    [Tooltip("controller grab input"), Header("Controller grab clicked")]
    public InputAction
        controller_grib;
    [Tooltip("Controller trigger input"), Header("Controller trigger clicked")]
    public InputAction
        controller_trigger;
    [Tooltip("Thumstick input"), Header("Thumstick")]
    public InputAction
        controller_thumbstick;
    [Tooltip("thumbstick pressed input"), Header("thumbstick pressed")]
    public InputAction
        controller_thumbstick_pressed;
    private InteractableWorking grabbed_object;
    private Joint joint;
    public LayerMask snapping_layers;
    [Tooltip("The rotation offset of the hand (used to make sure the hand is rotated to correspond with real world hand)")]
    public Quaternion offset;
    SnappingPoint snapablePoint;

    HandInteraction handInteraction;
    [HideInInspector]
    public List<InteractableWorking> interactables = new List<InteractableWorking>();
    public List<SnappingPoint> snapables = new List<SnappingPoint>();

    public void Setup()
    {
        rb = controller.GetComponent<Rigidbody>();
        hand = controller.transform.GetChild(0).gameObject;
        handInteraction = controller.GetComponent<HandInteraction>();

        controlle_position.Enable();
        controller_rotation.Enable();
        controller_grib.Enable();
        controller_trigger.Enable();
        controller_thumbstick.Enable();
        controller_thumbstick_pressed.Enable();

        controlle_position.performed += UpdatePos;
        controller_rotation.performed += UpdateRot;
        controller_grib.performed += Grab;
        controller_grib.canceled += Drop;
        controller_trigger.performed += Trigger;
        controller_trigger.canceled += ReleaseTrigger;
    }

    public void UpdatePos(InputAction.CallbackContext context)
    {
        rb.linearVelocity = ((Vector3.Scale(context.ReadValue<Vector3>(), body.transform.localScale) + body.transform.position - rb.position) * 20f);
        
        if(grabbed_object)
        {
            //Vector3 dir;
            //if (left_controller)
            //    dir = controller.transform.right;
            //else
            //    dir = -controller.transform.right;
            //RaycastHit hit;
            //if (Physics.Raycast(controller.transform.position, dir, out hit, 0.25f, snapping_layers))
            //{
            //    if(hit.transform.GetComponent<SnappingPoint>())
            //    {
            //        if (hit.transform.GetComponent<SnappingPoint>().holding_ids.Contains<string>(grabbed_object.id))
            //        {
            //            if(snapablePoint != hit.transform.GetComponent<SnappingPoint>()){
            //                if(grabbed_object.GetComponent<MeshFilter>())
            //                    hit.transform.GetComponent<SnappingPoint>().ShowExampleModel(grabbed_object.GetComponent<MeshFilter>().mesh,grabbed_object.transform.localScale);
            //                else
            //                {
            //                    MeshFilter mf = ((MeshFilter)grabbed_object.GetComponentInChildren(typeof(MeshFilter), false));
            //                    hit.transform.GetComponent<SnappingPoint>().ShowExampleModel(mf.mesh, mf.transform.localScale);
            //                }
            //                snapablePoint = hit.transform.GetComponent<SnappingPoint>();
            //            }
            //            return;
            //        }
            //    }
            //}
            //if(snapablePoint != null)
            //{
            //    snapablePoint.HideExampleModel();
            //    snapablePoint = null;
            //}
            if (snapables.Count > 0)
            {
                float dist = 1000;
                SnappingPoint ClosestSnappingPoint = null;
                foreach (SnappingPoint point in snapables)
                {
                    if (point.holding_ids.Contains<string>(grabbed_object.id))
                    {
                        if (Vector3.Distance(controller.transform.position, point.transform.position) < dist)
                        {
                            dist = Vector3.Distance(controller.transform.position, point.transform.position);
                            ClosestSnappingPoint = point;
                        }
                    }
                }
                if (snapablePoint != ClosestSnappingPoint)
                {
                    if (snapablePoint != null)
                    {
                        snapablePoint.HideExampleModel();
                        snapablePoint = null;
                    }
                    if (grabbed_object.GetComponent<MeshFilter>())
                        ClosestSnappingPoint.ShowExampleModel(grabbed_object.GetComponent<MeshFilter>().mesh, grabbed_object.transform.localScale);
                    else
                    {
                        MeshFilter mf = ((MeshFilter)grabbed_object.GetComponentInChildren(typeof(MeshFilter), false));
                        ClosestSnappingPoint.ShowExampleModel(mf.mesh, mf.transform.localScale);
                    }
                    snapablePoint = ClosestSnappingPoint;
                }
            }
            else if(snapablePoint != null)
            {
                snapablePoint.HideExampleModel();
                snapablePoint = null;
            }
        }
        else
        {
            //Vector3 dir;
            //if (left_controller)
            //    dir = controller.transform.right;
            ////dir = hand.transform.TransformDirection(Vector3.right);
            //else
            //    dir = -controller.transform.right;
            ////dir = hand.transform.TransformDirection(Vector3.left);
            //RaycastHit hit;
            //if (Physics.Raycast(controller.transform.position, dir, out hit, 0.1f) && hit.transform.GetComponent<Interactable>())
            //{
            //    if (controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Spread" && controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Fist")
            //    {
            //            controller_animator.SetTrigger("Spread");
            //    }
            //}
            //else if (controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Spread")
            //{
            //    controller_animator.SetTrigger("Idle");
            //}
            if (interactables.Count > 0)
            {
                if (controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Spread" && controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Fist")
                {
                    controller_animator.SetTrigger("Spread");
                }
            }
            else if (controller_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Spread")
            {
                controller_animator.SetTrigger("Idle");
            }
        }
    }

    public void UpdateRot(InputAction.CallbackContext context)
    {
        //add rotation offset
        //rotation is shit
        //like really shit
        //rb.angularVelocity = Vector3.RotateTowards(controller.transform.rotation.eulerAngles, context.ReadValue<Quaternion>().eulerAngles + offset.eulerAngles, 1, 1);
        //Debug.Log(Vector3.RotateTowards(controller.transform.rotation.eulerAngles, context.ReadValue<Quaternion>().eulerAngles + offset.eulerAngles, 1, 1));
        //rb.angularVelocity = Mathf.Deg2Rad * ((context.ReadValue<Quaternion>()).eulerAngles + offset.eulerAngles - controller.transform.rotation.eulerAngles);
        //Debug.Log((context.ReadValue<Quaternion>() * offset).eulerAngles + "\n" + controller.transform.rotation.eulerAngles + "\n" + ((context.ReadValue<Quaternion>() * offset).eulerAngles - controller.transform.rotation.eulerAngles));
        //if(grabbed_object == null)
        //    rb.MoveRotation(context.ReadValue<Quaternion>() * offset);//((context.ReadValue<Quaternion>() * offset) * (rb.rotation));
        //else if(grabbed_object.GetType().ToString() == "GrabableObject")
        //{
        //    rb.MoveRotation(context.ReadValue<Quaternion>() * offset);//((context.ReadValue<Quaternion>() * offset) * (rb.rotation));
        //    grabbed_object.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //}
        Quaternion deltaRotation = (context.ReadValue<Quaternion>() * offset) * Quaternion.Inverse(rb.rotation);
        float angle = 0.0f;
        Vector3 axis = Vector3.zero;
        deltaRotation.ToAngleAxis(out angle, out axis);
        angle *= Mathf.Deg2Rad;
        Vector3 angularVelocity = axis * angle;
        rb.angularVelocity = angularVelocity * 20;
    }

    public void Grab(InputAction.CallbackContext context)
    {
        //Vector3 dir;
        //if (left_controller)
        //    dir = controller.transform.right;
        //    //dir = hand.transform.TransformDirection(Vector3.right);
        //else
        //    dir = -controller.transform.right;
        //    //dir = hand.transform.TransformDirection(Vector3.left);
        //RaycastHit hit;
        //if (Physics.Raycast(controller.transform.position, dir, out hit, 0.1f) && hit.transform.GetComponent<Interactable>())
        //{
        //    grabbed_object = hit.transform.GetComponent<Interactable>().Grabbed(this);
        //    if(grabbed_object != null)
        //    {
        //        //Grip grab animation
        //        controller_animator.SetTrigger("GrabLarge");
        //        grabbed_object.on_grab.Invoke();
        //        joint = controller.AddComponent<FixedJoint>();
        //        joint.connectedBody = hit.transform.GetComponent<Rigidbody>();
        //    }
        //    else
        //    {
        //        //Grip fist animation
        //        controller_animator.SetTrigger("Fist");
        //        //Debug.Log("fist");
        //    }
        //}
        //else
        //{
        //    //Grip fist animation
        //    controller_animator.SetTrigger("Fist");
        //    //Debug.Log("fist");
        //}
        if (interactables.Count > 0)
        {
            float dist = 1000;
            InteractableWorking ClosestObject = null;
            foreach (InteractableWorking go in interactables)
            {
                if (Vector3.Distance(controller.transform.position, go.transform.position) < dist)
                {
                    dist = Vector3.Distance(controller.transform.position, go.transform.position);
                    ClosestObject = go.GetComponent<InteractableWorking>();
                }
            }
            grabbed_object = ClosestObject.Grabbed(this);
            if (grabbed_object != null)
            {
                //Grip grab animation
                controller_animator.SetTrigger("GrabLarge");
                grabbed_object.on_grab.Invoke();
                joint = controller.AddComponent<FixedJoint>();
                joint.connectedBody = ClosestObject.transform.GetComponent<Rigidbody>();
                handInteraction.IsHoldingObject = true;
            }
            else
            {
                //Grip fist animation
                controller_animator.SetTrigger("Fist");
                //Debug.Log("fist");
            }
        }
        else
        {
            //Grip fist animation
            controller_animator.SetTrigger("Fist");
            //Debug.Log("fist");
        }
    }

    public void Drop(InputAction.CallbackContext context)
    {
        //if (grabbed_object)
        //{
        //    //Grip idle animation
        //    controller_animator.SetTrigger("Idle");
        //    joint.connectedBody = null;
        //    Player.Destroy(joint);
        //    joint = null;
        //    Vector3 dir;
        //    if (left_controller)
        //        dir = controller.transform.right;
        //    else
        //        dir = -controller.transform.right;
        //    RaycastHit hit;
        //    if (Physics.Raycast(controller.transform.position, dir, out hit, 0.25f, snapping_layers) && hit.transform.GetComponent<SnappingPoint>())
        //    {
        //        grabbed_object.Snap(hit.transform.GetComponent<SnappingPoint>());
        //        if (snapablePoint != null)
        //        {
        //            snapablePoint.HideExampleModel();
        //            snapablePoint = null;
        //        }
        //    }
        //    grabbed_object.Dropped(rb.velocity * 2);
        //    grabbed_object.on_drop.Invoke();
        //    grabbed_object = null;
        //    rb.angularVelocity= Vector3.zero;
        //    handInteraction.IsHoldingObject = false;
        //}
        //else
        //{
        //    //Grip idle animation
        //    controller_animator.SetTrigger("Idle");
        //}
        if (grabbed_object)
        {
            //Grip idle animation
            controller_animator.SetTrigger("Idle");
            joint.connectedBody = null;
            Player.Destroy(joint);
            joint = null;
            if (snapables.Count > 0)
            {
                float dist = 1000;
                SnappingPoint ClosestSnappingPoint = null;
                foreach (SnappingPoint point in snapables)
                {
                    if (point.holding_ids.Contains<string>(grabbed_object.id))
                    {
                        if (Vector3.Distance(controller.transform.position, point.transform.position) < dist)
                        {
                            dist = Vector3.Distance(controller.transform.position, point.transform.position);
                            ClosestSnappingPoint = point;
                        }
                    }
                }
                if (ClosestSnappingPoint != null)
                {
                    grabbed_object.Snap(ClosestSnappingPoint);
                    if (snapablePoint != null)
                    {
                        snapablePoint.HideExampleModel();
                        snapablePoint = null;
                    }
                }
            }
            grabbed_object.Dropped(rb.linearVelocity * 2);
            grabbed_object.on_drop.Invoke();
            grabbed_object = null;
            rb.angularVelocity = Vector3.zero;
            handInteraction.IsHoldingObject = false;
        }
        else
        {
            //Grip idle animation
            controller_animator.SetTrigger("Idle");
        }
    }

    public void DeatachObject()
    {
        if (grabbed_object)
        {
            //idle animation
            joint.connectedBody = null;
            Player.Destroy(joint);
            joint = null;
            grabbed_object = null;
            controller_animator.SetTrigger("Fist");
        }
        else
        {
            Debug.LogError("God is dead, we killed him \n controller has nothing to deatach");
        }
    }

    public void AttachObject(InteractableWorking object_to_attach)
    {
        joint = controller.AddComponent<FixedJoint>();
        joint.connectedBody = object_to_attach.transform.GetComponent<Rigidbody>();
        grabbed_object = object_to_attach.Grabbed(this);
        controller_animator.SetTrigger("GrabLarge");
    }

    public void Trigger(InputAction.CallbackContext context)
    {
        if (grabbed_object)
        {
            //index_use animation
            grabbed_object.on_trigger.Invoke();
        }
        else
        {
            //index_fold animation
        }
    }

    public void ReleaseTrigger(InputAction.CallbackContext context)
    {
        if (grabbed_object)
        {
            grabbed_object.on_trigger_release.Invoke();
        }
        else
        {
            //index_idle animation
        }
    }
}