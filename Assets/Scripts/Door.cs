using UnityEngine;
public class Door : Interactable
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void Activate()
    {
        anim.SetBool("Open", true);
    }

    public override void DeActivate()
    {
        anim.SetBool("Open", false);
    }
}