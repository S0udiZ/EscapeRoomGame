using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{

    [SerializeField]
    private Collider pickUpCollider, SnapColllider;
    [SerializeField]
    Player player;
    [SerializeField]
    bool isRigtHand;
    bool isHoldingObject;
    public bool IsHoldingObject 
    {  
        get 
        { 
            return isHoldingObject;
        }  
        set 
        { 
            isHoldingObject = value;
            if (isRigtHand)
            {
                player.r_hand.snapables.Clear();
                player.r_hand.interactables.Clear();
            }
            else
            {
                player.l_hand.snapables.Clear();
                player.l_hand.interactables.Clear();
            }
            if (value == true)
            {
                pickUpCollider.enabled = false;
                SnapColllider.enabled = true;
            }
            else
            {
                pickUpCollider.enabled = true;
                SnapColllider.enabled = false;
            }
        } 
    }

    public void Start()
    {
        pickUpCollider.enabled = true;
        SnapColllider.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isRigtHand)
        {
            if (isHoldingObject)
            {
                if (other.GetComponent<SnappingPoint>())
                    player.r_hand.snapables.Add(other.GetComponent<SnappingPoint>());
            }
            else
            {
                if (other.GetComponent<InteractableWorking>())
                    player.r_hand.interactables.Add(other.GetComponent<InteractableWorking>());
            }
        }
        else
        {
            if (isHoldingObject)
            {
                if (other.GetComponent<SnappingPoint>())
                    player.l_hand.snapables.Add(other.GetComponent<SnappingPoint>());
            }
            else
            {
                if (other.GetComponent<InteractableWorking>())
                    player.l_hand.interactables.Add(other.GetComponent<InteractableWorking>());
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isRigtHand)
        {
            if (isHoldingObject)
            {
                if (other.GetComponent<SnappingPoint>())
                    player.r_hand.snapables.Remove(other.GetComponent<SnappingPoint>());
            }
            else
            {
                if (other.GetComponent<InteractableWorking>())
                    player.r_hand.interactables.Remove(other.GetComponent<InteractableWorking>());
            }
        }
        else
        {
            if (isHoldingObject)
            {
                if (other.GetComponent<SnappingPoint>())
                    player.l_hand.snapables.Remove(other.GetComponent<SnappingPoint>());
            }
            else
            {
                if (other.GetComponent<InteractableWorking>())
                    player.l_hand.interactables.Remove(other.GetComponent<InteractableWorking>());
            }
        }
    }
}
