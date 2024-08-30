using System;
using UnityEngine;

public class HintControl : MonoBehaviour
{
    public Animator anim;

    public event Action btnPressed;
    public event Action triggerEnter;
    public event Action triggerExit;

    public SpriteRenderer blackoutPressed;
    public float fillRatePressed;
    float fillPressed;

    float blackoutPressedStartSize;

    bool isWork = true;

    [SerializeField] private InputController input;
    private bool btnPress;

    private void Awake()
    {
        blackoutPressedStartSize = blackoutPressed.size.x;

        input.controls.InteractionBtn.Press.performed += context => btnPress = true;
        input.controls.InteractionBtn.Press.canceled += context => btnPress = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isWork)
        {
            triggerEnter?.Invoke();

            anim.gameObject.SetActive(true);
            anim.SetBool("show", true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        bool isShow = (anim.GetCurrentAnimatorStateInfo(0).IsName("Show") || anim.GetCurrentAnimatorStateInfo(0).IsName("Hide")) ? (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) ? true : false : false;

        if (other.tag == "Player" && btnPress && isWork && !isShow)
        {
            anim.SetBool("pressed", true);
            if (fillPressed + fillRatePressed < 100)
            {
                fillPressed += fillRatePressed;
            }
            else
            {
                fillPressed = 100;
            }
        }
        if (other.tag == "Player" && !btnPress && isWork && !isShow)
        {
            anim.SetBool("pressed", false);
            if (fillPressed - (fillRatePressed * 1.5f) > 0)
            {
                fillPressed -= fillRatePressed * 2f;
            }
            else
            {
                fillPressed = 0;
            }
        }

        blackoutPressed.size = new Vector2(blackoutPressedStartSize, blackoutPressedStartSize * fillPressed / 100);
        float hintSize = 1 - fillPressed / 650;
        transform.localScale = new Vector2(hintSize * Mathf.Sign(transform.localScale.x), hintSize);

        if (fillPressed >= 100 && isWork)
        {
            anim.SetBool("show", false);
            anim.SetBool("pressed", false);

            btnPress = false;
            btnPressed?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && isWork)
        {
            triggerExit?.Invoke();

            anim.SetBool("show", false);
            anim.SetBool("pressed", false);
        }
    }

    public void SwitchStateWork()
    {
        isWork = !isWork;

        if (isWork == false)
        {
            anim.SetBool("show", false);
            anim.SetBool("pressed", false);
        }
    }
}