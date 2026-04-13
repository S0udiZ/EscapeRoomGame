using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] Transform ConnectedBody;
    [SerializeField] float RestingDistance;
    [SerializeField] float SpringForce;
    [SerializeField] float Dampaning;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject} Does not have a rigidbody asigned");
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = (ConnectedBody.position - transform.position).normalized;
        Vector3 targetPos = -direction * RestingDistance + ConnectedBody.position;

        Vector3 targetForce = (targetPos - transform.position) * SpringForce;

        rb.AddForce(targetForce);

        Vector3 dampForce = rb.linearVelocity * -Dampaning;

        rb.AddForce(dampForce);

    }

}
