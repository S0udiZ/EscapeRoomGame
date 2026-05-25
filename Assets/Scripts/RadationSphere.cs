using System;
using UnityEngine;

public class RadationSphere : MonoBehaviour
{
    [NonSerialized]
    public Transform Hand;
    [NonSerialized]
    public bool Reversed = false;
    bool? isReversed = null;
    LineRenderer lineRenderer;

    [SerializeField]
    GameObject Particles, ReverseParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Hand)
        {
            return;
        }
        Vector3 dif = Hand.position - transform.position;
        lineRenderer.SetPosition(1, 1 / transform.lossyScale.x * dif);

        if (isReversed != Reversed)
        {
            if (Reversed == true)
            {
                ReverseParticles.SetActive(true);
                Particles.SetActive(false);
                isReversed = true;
            }
            else
            {
                ReverseParticles.SetActive(false);
                Particles.SetActive(true);
                isReversed = false;
            }
        }

    }
}
