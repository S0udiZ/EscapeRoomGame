using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.OpenXR.Input;

[RequireComponent(typeof(Rigidbody))]
public abstract class InteractableWorking : MonoBehaviour
{

    [Tooltip("id is used by other components such as snapping point to target only object with that id")]
    public string id;
    [Tooltip("Breaking variables")]
    public Breaking Breaking;
    private Rigidbody rb;
    [HideInInspector]
    public Controller holding_controller = null;
    public SnappingPoint snapped_point;

    public UnityEvent on_grab, on_drop, on_trigger, on_trigger_release;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// sets all variables up when grabbed
    /// </summary>
    /// <param name="controller">the controller thats grapping the object</param>
    public InteractableWorking Grabbed(Controller controller)
    {
        if (snapped_point)
        {
            if (snapped_point.holster)
            {
                DeSnap();
                holding_controller = controller;
                return this;
            }
            else
            {
                if (snapped_point.GetComponent<InteractableWorking>())
                {
                    return snapped_point.GetComponent<InteractableWorking>().Grabbed(controller); 
                }
                else
                {
                    return null;
                }
            }
        }
        else
        {
            holding_controller = controller;
            return this;
        }
    }

    public void Dropped(Vector3 velocety)
    {
        holding_controller = null;
        rb.linearVelocity = velocety;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Breaking.break_force != 0)
        {
            Break(collision);
        }
    }

    /// <summary>
    /// hella awesome item breaking
    /// </summary>
    /// <param name="collision">the collision information</param>
    private void Break(Collision collision)
    {
        if(collision.impulse.magnitude > Breaking.break_force)
        {
            if(holding_controller != null)
            {
                holding_controller.DeatachObject();
            }
            if(Breaking.break_drops.Length > 0)
            {
                for (int i = 0; i < Breaking.break_drops.Length; i++)
                {
                    GameObject inst = Instantiate(Breaking.break_drops[i].game_object, transform.position + Breaking.break_drops[i].offset, Quaternion.Euler(transform.eulerAngles + Breaking.break_drops[i].rotation_offset));
                    inst.GetComponent<Rigidbody>().AddExplosionForce(10 * collision.impulse.magnitude, collision.contacts[0].point,1);
                }
            }
            Breaking.on_break.Invoke();
            Destroy(gameObject);
        }
    }

    public void ForceDrop()
    {
        holding_controller.DeatachObject();
        holding_controller = null;
    }

    public void Snap(SnappingPoint snapping_point)
    {
        if (snapping_point.holding_ids.Contains<string>(id))
        {
            snapped_point = snapping_point;
            snapped_point.Snap(gameObject);
        }
    }

    public void DeSnap()
    {
        snapped_point.Release(gameObject);
        snapped_point=null;
    }
}

[System.Serializable]
public class Breaking
{
    [Tooltip("how much force is needed to break the item\n0 = unbreakable")]
    public float break_force;
    [Tooltip("the items that will be dropped if broken")]
    public Drop[] break_drops;
    public UnityEvent on_break;
}

[System.Serializable]
public class Drop
{

    public GameObject game_object;
    public Vector3 offset;
    public Vector3 rotation_offset;

}