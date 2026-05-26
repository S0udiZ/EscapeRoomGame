using UnityEngine;

public class Poster : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private Controller hand;

    private void Start()
    {
        interactionPoint = GetComponentInChildren<GameObject>().transform;
    }

    public void OnGrabbingPoster()
    {

    }
}
