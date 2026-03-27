using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public Transform resetOrigin;

    public InputActionProperty testActionButton;

    void Update()
    {
        bool button = testActionButton.action.IsPressed();

        if (button)
        {
            ResetPosition(); 
        } 
    }

    void ResetPosition()
    {
        this.transform.position = resetOrigin.position;
    }
}
