using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public InputSettings controls { private set; get; }

    private void Awake()
    {
        Instance = this;
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
