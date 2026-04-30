using Oculus.Interaction;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] Transform ConnectedBody;
    [SerializeField] float RestingDistance = 0.5f;
    [SerializeField] float stopDistance = 0.001f;
    [SerializeField] bool AutoSetRestingDistance = false;
    [SerializeField] float SpringForce = 55;
    [SerializeField] float Dampaning = 10;

    [SerializeField] float MaxLength;
    [SerializeField] bool OneDirectonal;


    Vector3 movDirection;
    Vector3 startPos;
    Rigidbody rb;
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{gameObject} Does not have a rigidbody asigned");
        }
        if (AutoSetRestingDistance)
        {
            RestingDistance = (transform.position - ConnectedBody.position).magnitude;
        }
        movDirection = (transform.position - ConnectedBody.position).normalized;
    }

    void FixedUpdate()
    {

        Vector3 direction = (ConnectedBody.position - transform.position).normalized;
        Vector3 targetPos = -direction * RestingDistance + ConnectedBody.position;

        if ((targetPos - transform.position).sqrMagnitude <= stopDistance * stopDistance)
        {
            return;
        }

        Vector3 targetForce = (targetPos - transform.position) * SpringForce;

        rb.AddForce(targetForce);

        Vector3 dampForce = rb.linearVelocity * -Dampaning;

        rb.AddForce(dampForce);

        MovmentConstraints();



    }

    void MovmentConstraints()
    {
        if (MaxLength > 0)
        {
            Vector3 diffVec = transform.position - ConnectedBody.position;

            if (diffVec.sqrMagnitude > MaxLength * MaxLength)
            {
                Vector3 clampedDiff = Vector3.ClampMagnitude(diffVec, MaxLength);
                transform.position = ConnectedBody.position + clampedDiff;
            }
        }


        if (OneDirectonal)
        {
            float VecLength = (transform.position - ConnectedBody.position).magnitude;
            Vector3 newDiff = VecLength * movDirection;
            transform.position = ConnectedBody.position + newDiff;
        }
    }

}
