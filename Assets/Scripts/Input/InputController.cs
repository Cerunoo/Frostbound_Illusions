using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public InputSettings controls { private set; get; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance == this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

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
