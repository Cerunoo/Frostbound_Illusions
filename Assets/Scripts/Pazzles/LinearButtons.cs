using UnityEngine;
using UnityEngine.UI;

public class LinearButtons : MonoBehaviour
{
    public Animator[] buttons;
    public float[] speedsPress;

    private bool[] pressedButtons = new bool[3];
    private bool passed;

    private void Awake()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Interactions.Pazzle1.performed += context =>
            {
                if (!CanPressButton(0)) return;
                buttons[0].SetBool("Press", true);
                buttons[0].SetFloat("SpeedPress", speedsPress[0]);
            };
            InputController.Instance.controls.Interactions.Pazzle2.performed += context =>
            {
                if (!CanPressButton(1)) return;
                buttons[1].SetBool("Press", true);
                buttons[1].SetFloat("SpeedPress", speedsPress[1]);
            };
            InputController.Instance.controls.Interactions.Pazzle3.performed += context =>
            {
                if (!CanPressButton(2)) return;
                buttons[2].SetBool("Press", true);
                buttons[2].SetFloat("SpeedPress", speedsPress[2]);
            };

            InputController.Instance.controls.Interactions.Pazzle1.canceled += context =>
            {
                buttons[0].SetBool("Press", false);
            };
            InputController.Instance.controls.Interactions.Pazzle2.canceled += context =>
            {
                buttons[1].SetBool("Press", false);
            };
            InputController.Instance.controls.Interactions.Pazzle3.canceled += context =>
            {
                buttons[2].SetBool("Press", false);
            };
        }
    }

    private void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            pressedButtons[i] = buttons[i].GetComponent<Image>().fillAmount <= 0;
        }

        if (!passed && pressedButtons[2] == true)
        {
            GetComponent<Animator>().SetTrigger("Hide");
            GetComponent<PazzleController>().InvokePassedAction();
            passed = true;
        }
    }

    private bool CanPressButton(int indexButton)
    {
        for (int i = 0; i < pressedButtons.Length; i++)
        {
            if (pressedButtons[i] == false)
            {
                if (i == indexButton) return true;
                else return false;
            };
        }
        return false;
    }
}