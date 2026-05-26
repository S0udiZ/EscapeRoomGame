using UnityEngine;

public class KeyHole : MonoBehaviour
{
    public bool IsActive = false;
    [SerializeField] KeyHole OtherKeyHole;

    [SerializeField] PrototypeDoor door;

    void Start()
    {
        if (OtherKeyHole == null)
        {
            Debug.LogError("OtherKeyHole is not assigned in the inspector.");
        }
    }
    void FixedUpdate()
    {
        if (transform.localRotation.eulerAngles.z > 0f)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (transform.localRotation.eulerAngles.z < -90f)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        }
        if (transform.localRotation.eulerAngles.z == -90f)
        {
            IsActive = true;
        }
        else
        {
            IsActive = false;
        }

        if (IsActive && OtherKeyHole != null && OtherKeyHole.IsActive)
        {
            door.Activate();
        }
    }
}
