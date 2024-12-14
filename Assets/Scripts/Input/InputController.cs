using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    public static InputController Instance { get; private set; }
    public InputSettings controls { private set; get; }

    [SerializeField] private List<InputAction> disabledActions;
    
    private void Awake()
    {
        Instance = this;
        controls = new InputSettings();

        disabledActions = new List<InputAction>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public static void DisableAllInput()
    {
        Instance.controls.Disable();
    }
    public static void EnableAllInput()
    {
        Instance.controls.Enable();
        foreach (InputAction action in Instance.disabledActions) action.Disable();
    }

    public static void DisableInput(InputAction action)
    {
        action.Disable();
        Instance.disabledActions.Add(action);
    }
    public static void EnableInput(InputAction action)
    {
        action.Enable();
        Instance.disabledActions.RemoveAll(p => p == action);
    }

    public static void DisableInput(InputAction[] actions)
    {
        foreach(InputAction action in actions)
        {
            action.Disable();
            Instance.disabledActions.Add(action);
        }
    }
    public static void EnableInput(InputAction[] actions)
    {
        foreach(InputAction action in actions)
        {
            action.Enable();
            Instance.disabledActions.RemoveAll(p => p == action);
        }
    }
}