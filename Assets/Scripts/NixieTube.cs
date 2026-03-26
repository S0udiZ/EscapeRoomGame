using UnityEngine;
public class NixieTube : MonoBehaviour
{
    [SerializeField] private int numb = 0;
    [SerializeField] private TMPro.TextMeshPro[] text;

    [ContextMenu("Increment")]
    public void Increment()
    {
        numb++;
        if (numb > 9) numb = 0;
        foreach (var t in text)
        {
            t.text = numb.ToString();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var t in text)
        {
            t.text = numb.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
