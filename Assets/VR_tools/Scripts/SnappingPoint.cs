using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class SnappingPoint : MonoBehaviour
{

    public string[] holding_ids;
    public bool holster;
    public Vector3 snap_offset;
    public GameObject snapped_object;
    public UnityEvent on_snapped;
    public UnityEvent on_released;
    public Material objectShowcaseMaterial;
    Joint joint;
    GameObject modelshowcase;

    public void ShowExampleModel(Mesh modelMesh, Vector3 scale)
    {
        if(modelshowcase == null)
        {
            modelshowcase = new GameObject();
            modelshowcase.transform.position = transform.position + snap_offset;
            modelshowcase.transform.parent = transform;
            modelshowcase.AddComponent<MeshRenderer>();
            modelshowcase.GetComponent<MeshRenderer>().material = objectShowcaseMaterial;
            modelshowcase.AddComponent<MeshFilter>();
        }
        modelshowcase.transform.localScale = new Vector3(scale.x / transform.localScale.x, scale.y / transform.localScale.y, scale.z / transform.localScale.z);
        modelshowcase.GetComponent<MeshFilter>().mesh = modelMesh;
        modelshowcase.SetActive(true);
    }

    public void HideExampleModel()
    {
        modelshowcase.SetActive(false);
    }

    public void Snap(GameObject interactable)
    {
        snapped_object = interactable;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.enableCollision = false;
        interactable.transform.position = transform.position + transform.forward * snap_offset.z + transform.up * snap_offset.y + transform.right * snap_offset.x;
        joint.connectedBody = interactable.GetComponent<Rigidbody>();
        on_snapped.Invoke();
    }

    public void Release(GameObject interactable)
    {
        snapped_object = null;
        joint.connectedBody = null;
        Destroy(joint);
        on_released.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position + snap_offset, 0.1f);
    }
#endif
}