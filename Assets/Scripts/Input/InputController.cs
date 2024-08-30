using UnityEngine;

public class InputController : MonoBehaviour
{
    public InputSettings controls { private set; get; }

    private void Awake()
    {
        controls = new InputSettings();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
