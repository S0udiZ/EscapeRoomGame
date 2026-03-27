using UnityEngine;
public class NixieTube : MonoBehaviour
{
    [SerializeField] public int numb = 0;
    [SerializeField] public TMPro.TextMeshPro[] text;
    public bool Disabled;

    [ContextMenu("Increment")]
    public void Increment()
    {
        if (Disabled)
        {
            return;
        }
        numb++;
        if (numb > 9) numb = 0;
        SetText();
    }

    public void SetText()
    {
        foreach (var t in text)
        {
            t.text = numb.ToString();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetText();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
