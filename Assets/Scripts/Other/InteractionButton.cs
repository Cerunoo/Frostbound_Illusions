using System;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    public Action _btnPress;
    public event Action btnPress
    {
        add
        {
            _btnPress += value;
            nullListeners = false;
        }
        remove
        {
            _btnPress -= value;
            if (_btnPress == null) nullListeners = true;
        }
    }
    private bool nullListeners;
    public event Action btnExit;
    private bool inTrigger;
    [HideInInspector] public bool pressed;

    [SerializeField] private Animator anim;

    private bool isDisable = false;
    public bool IsDisable
    {
        get => isDisable;
        set
        {
            isDisable = value;
            if (isDisable == true)
            {
                anim.SetBool("Show", false);
                anim.SetBool("Pressed", false);
            }
        }
    }

    private void Awake()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Interactions.Call.performed += context =>
            {
                if (inTrigger && !IsDisable && !nullListeners && !pressed)
                {
                    anim.SetBool("Show", false);
                    anim.SetBool("Pressed", true);

                    pressed = true;
                    _btnPress?.Invoke();
                }
            };
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !IsDisable && !nullListeners)
        {
            anim.SetBool("Show", true);
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !IsDisable && !nullListeners)
        {
            anim.SetBool("Show", false);
            anim.SetBool("Pressed", false);
            inTrigger = false;

            btnExit?.Invoke();
        }
    }

    public static void StaticDisable(InteractionButton button)
    {
        button.IsDisable = true;
    }

    public static void StaticEnable(InteractionButton button)
    {
        button.IsDisable = false;
    }
}