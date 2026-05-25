using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Lock : Interactable
{
    [SerializeField]
    Color StartColor, EndColor;
    [SerializeField]
    float DisolveTime;
    Renderer MeshRenderer;

    [SerializeField]
    bool Debug;
    bool running = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug)
        {
            Debug = false;
            Activate();
        }
    }

    public override void Activate()
    {
        if (running)
        {
            return;
        }
        running = true;
        StartCoroutine(Disolve());
    }

    IEnumerator Disolve()
    {
        float curTime = 0;
        while (curTime < DisolveTime)
        {
            float ratio = curTime / DisolveTime;
            Color newColor = Color.Lerp(StartColor, EndColor, ratio);
            MeshRenderer.material.color = newColor;

            curTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        Destroy(this);

    }
}
