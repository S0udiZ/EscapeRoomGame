using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    [SerializeField]
    Interactable[] Doors;
    [SerializeField]
    float distance;
    [SerializeField]
    GameObject Player;
    bool active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 a = new(Player.transform.position.x, Player.transform.position.z);
        Vector2 b = new(transform.position.x, transform.position.z);
        if (distance > Vector3.Distance(a, b))
        {
            active = true;
        }
        if (active)
        {
            foreach (var item in Doors)
            {
                item.Activate();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {


    }
}
