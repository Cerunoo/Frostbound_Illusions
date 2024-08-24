using System;
using UnityEngine;
using UnityEngine.UI;

public class HintControl : MonoBehaviour
{
    public KeyCode keycodeEvent;
    public Animator anim;

    public event Action keyPressed;
    public event Action triggerEnter;
    public event Action triggerExit;

    public Image blackoutPressed;
    public float fillRatePressed;
    float fillPressed;

    bool isWork = true;

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

        if (other.tag == "Player" && Input.GetKey(keycodeEvent) && isWork && !isShow)
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
        if (other.tag == "Player" && !Input.GetKey(keycodeEvent) && isWork && !isShow)
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

        blackoutPressed.fillAmount = fillPressed / 100;
        float hintSize = 1 - fillPressed / 650;
        anim.gameObject.GetComponent<RectTransform>().localScale = new Vector2(hintSize * Mathf.Sign(anim.gameObject.GetComponent<RectTransform>().localScale.x), hintSize);

        if (fillPressed >= 100 && isWork)
        {
            anim.SetBool("show", false);
            anim.SetBool("pressed", false);

            keyPressed?.Invoke();
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