using UnityEngine;

public class Toutrure : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new(0, 0, speed * Time.deltaTime));
    }
}
