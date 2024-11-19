using UnityEngine;
using UnityEngine.UI;

public class LinearButtons : MonoBehaviour
{
    public Animator[] buttonsYHB;
    [SerializeField] private float speedPress;

    private bool startY;
    private bool saveH;
    private bool saveB;

    private bool failH;
    private bool failB;

    private bool passed;

    private void Awake()
    {
        buttonsYHB[0].SetFloat("Speed", speedPress);
        buttonsYHB[1].SetFloat("Speed", speedPress);
        buttonsYHB[2].SetFloat("Speed", speedPress);

        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Interactions.Pazzle1.performed += context =>
            {
                if (startY) return;
                buttonsYHB[0].SetBool("Press", true);
                startY = true;
            };
            InputController.Instance.controls.Interactions.Pazzle2.performed += context =>
            {
                if (failH) return;
                if (buttonsYHB[1].GetComponent<Image>().fillAmount == 0.99f || buttonsYHB[1].GetBool("Press") == false || !startY || saveH)
                {
                    failH = true;
                    return;
                }
                saveH = true;
            };
            InputController.Instance.controls.Interactions.Pazzle3.performed += context =>
            {
                if (failB) return;
                if (buttonsYHB[2].GetComponent<Image>().fillAmount == 0.99f || buttonsYHB[2].GetBool("Press") == false || !saveH || saveB)
                {
                    failB = true;
                    return;
                }
                saveB = true;
            };
        }
    }

    private void Update()
    {
        if (buttonsYHB[0].GetComponent<Image>().fillAmount == 0.99f)
        {
            buttonsYHB[1].SetBool("Press", true);
        }
        if (buttonsYHB[1].GetComponent<Image>().fillAmount == 0.99f)
        {
            buttonsYHB[2].SetBool("Press", true);
        }

        if (!passed && buttonsYHB[2].GetComponent<Image>().fillAmount == 0.99f)
        {
            if (saveH && saveB)
            {
                GetComponent<Animator>().SetTrigger("Hide");
                GetComponent<PazzleController>().InvokePassedAction();
                passed = true;
            }
            else
            {
                if (!saveH) buttonsYHB[1].SetBool("Unpress", true);
                if (!saveB) buttonsYHB[2].SetBool("Unpress", true);
            }
        }

        if (!passed && (buttonsYHB[1].GetComponent<Image>().fillAmount == 0.98f || buttonsYHB[2].GetComponent<Image>().fillAmount == 0.98f))
        {
            GetComponent<Animator>().SetTrigger("Hide");
            GetComponent<PazzleController>().InvokeFailedAction();
            passed = true;
        }
    }
}