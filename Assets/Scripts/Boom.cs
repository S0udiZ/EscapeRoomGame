using UnityEngine;

public class Boom : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] bool button;
    [SerializeField] int num;
    [SerializeField] int Count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (button)
        {
            button = false;
            for (int i = 0; i < num; i++)
            {
                Count++;
                float x = Random.Range(-0.1f, 0.1f);
                float y = Random.Range(-0.1f, 0.1f);
                float z = Random.Range(-0.1f, 0.1f);
                Vector3 ofset = new(x, y, z);
                Instantiate(obj, transform.position + ofset, transform.rotation);
            }
        }
    }
}
