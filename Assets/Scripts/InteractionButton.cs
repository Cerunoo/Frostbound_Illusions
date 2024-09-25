using System;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    public event Action btnPress;
    public event Action btnExit;
    private bool inTrigger;

    private Animator anim;

    private bool isDisable = false;
    public bool IsDisable
    {
        get => isDisable;
        set
        {
            isDisable = !isDisable;
            if (isDisable == true)
            {
                anim.SetBool("Show", false);
                anim.SetBool("Pressed", false);
            }
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (InputController.Instance != null)
        {
            InputController.Instance.controls.InteractionButton.Press.performed += context =>
            {
                if (inTrigger && !IsDisable)
                {
                    anim.SetBool("Show", false);
                    anim.SetBool("Pressed", true);

                    btnPress?.Invoke();
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