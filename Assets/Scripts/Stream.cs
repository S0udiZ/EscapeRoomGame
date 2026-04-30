using UnityEngine;

public class Stream : MonoBehaviour
{
    [SerializeField] Transform Tip;
    [SerializeField] float FlowRate = 1;
    [SerializeField] float MaxDistance = 10;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float StartWidth = 0.1f;
    [SerializeField] float WidthGrowth;

    [SerializeField] float PourAngle;

    [SerializeField] bool Button;

    bool flowing = true;
    LineRenderer lineRenderer;
    Vector3 colPos;
    float curDistance;
    // Start is called nce before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flowing)
        {
            RuningFlow();
        }


    }

    void RuningFlow()
    {
        if (curDistance < MaxDistance)
        {
            curDistance += FlowRate * Time.deltaTime;
            if (curDistance > MaxDistance)
            {
                curDistance = MaxDistance;
            }
        }


        RaycastHit hit;
        if (!Physics.Raycast(Tip.position, Vector3.down, out hit, curDistance, layerMask))
        {
            colPos = Vector3.down * curDistance;
        }
        else
        {
            colPos = hit.point;
        }
        float distance = Vector3.Distance(Tip.position, colPos);
        float endWidth = StartWidth + distance * WidthGrowth;

        lineRenderer.SetPosition(0, Tip.position);
        lineRenderer.SetPosition(1, colPos);

        lineRenderer.startWidth = StartWidth;
        lineRenderer.endWidth = endWidth;
    }

    void StoppingFlow()
    {

    }
}
