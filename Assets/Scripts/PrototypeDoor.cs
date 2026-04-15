using System.Collections;
using Meta.XR.MRUtilityKit.SceneDecorator;
using UnityEngine;

public class PrototypeDoor : Interactable
{
    [SerializeField] private Vector3 targetPosition;
    private Vector3 initialPosition;

    [SerializeField] bool debugopen;
    [SerializeField] bool debugclose;
    private bool isOpen = false;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (debugopen)
        {
            Activate();
            debugopen = false;
        }

        if (debugclose)
        {
            DeActivate();
            debugclose = false;
        }
    }

    private IEnumerator LerpToTarget()
    {
        float elapsedTime = 0f;
        float duration = 1f; // Duration of the animation

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition; // Ensure it ends exactly at the target
    }

    private IEnumerator LerpToInitial()
    {
        float elapsedTime = 0f;
        float duration = 1f; // Duration of the animation

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(targetPosition, initialPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition; // Ensure it ends exactly at the initial
    }

    public override void Activate()
    {
        if (isOpen) return;
        isOpen = true;
        StartCoroutine(LerpToTarget());
    }

    public override void DeActivate()
    {
        if (!isOpen) return;
        isOpen = false;
        StartCoroutine(LerpToInitial());
    }
}
