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
            IsDisable = false;
        }
        remove
        {
            _btnPress -= value;
            if (_btnPress == null) IsDisable = true;
        }
    }
    public event Action btnExit;
    private bool inTrigger;
    public bool pressed;

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
            InputController.Instance.controls.InteractionButton.Press.performed += context =>
            {
                if (inTrigger && !IsDisable && !pressed)
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
        if (other.tag == "Player" && !IsDisable)
        {
            anim.SetBool("Show", true);
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && !IsDisable)
        {
            anim.SetBool("Show", false);
            anim.SetBool("Pressed", false);
            inTrigger = false;

            btnExit?.Invoke();
        }
    }
}