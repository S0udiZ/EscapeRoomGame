using UnityEngine;

public class Stream : MonoBehaviour
{
    [SerializeField] Transform Tip;
    [SerializeField] float MaxDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] bool Button;
    LineRenderer lineRenderer;
    // Start is called nce before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, MaxDistance, layerMask)) { print("failed"); return; }

        Vector3 colPos = hit.collider.transform.position;

        print(colPos);

        lineRenderer.SetPosition(0, Tip.position);
        lineRenderer.SetPosition(1, colPos);


    }
}
