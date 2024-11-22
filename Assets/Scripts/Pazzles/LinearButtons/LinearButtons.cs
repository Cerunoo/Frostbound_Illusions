using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LinearButtons : MonoBehaviour
{
    [SerializeField] private LinButton[] buttons;

    [Space(5)]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform instParent;
    [SerializeField] private Transform[] posPoints;
    [SerializeField] private Sprite[] buttonSpritesYHB;

    private Queue<LinButton.KeyButton> queueKeys;
    private Animator[] anims;
    private bool qteStarted;

    private bool passed;

    private void Awake()
    {
        queueKeys = new Queue<LinButton.KeyButton>();
        foreach (LinButton button in buttons) queueKeys.Enqueue(button.key);

        anims = new Animator[buttons.Length];

        CreateButtons();
        BindInput();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Transform posPoint;
            switch (buttons[i].pos)
            {
                case LinButton.Pos.L_Bottom:
                    posPoint = posPoints[0];
                    break;
                case LinButton.Pos.L_Center:
                    posPoint = posPoints[1];
                    break;
                case LinButton.Pos.L_Top:
                    posPoint = posPoints[2];
                    break;
                case LinButton.Pos.C_Bottom:
                    posPoint = posPoints[3];
                    break;
                case LinButton.Pos.C_Center:
                    posPoint = posPoints[4];
                    break;
                case LinButton.Pos.C_Top:
                    posPoint = posPoints[5];
                    break;
                case LinButton.Pos.R_Bottom:
                    posPoint = posPoints[6];
                    break;
                case LinButton.Pos.R_Center:
                    posPoint = posPoints[7];
                    break;
                case LinButton.Pos.R_Top:
                    posPoint = posPoints[8];
                    break;
                default:
                    posPoint = posPoints[0];
                    break;
            }
            Vector2 pos = posPoint.position;
            Quaternion rot = posPoint.rotation;

            GameObject button = Instantiate(buttonPrefab, pos, rot, instParent);
            anims[i] = button.GetComponent<Animator>();

            Sprite spriteButton;
            switch(buttons[i].key)
            {
                case LinButton.KeyButton.Y:
                    spriteButton = buttonSpritesYHB[0];
                    break;
                case LinButton.KeyButton.H:
                    spriteButton = buttonSpritesYHB[1];
                    break;
                case LinButton.KeyButton.B:
                    spriteButton = buttonSpritesYHB[2];
                    break;
                default:
                    spriteButton = buttonSpritesYHB[0];
                    break;
            }
            button.GetComponent<Image>().sprite = spriteButton;
        }
    }

    private void BindInput()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Interactions.Pazzle1.performed += context =>
            {
                HandleInput(LinButton.KeyButton.Y);
            };
            InputController.Instance.controls.Interactions.Pazzle2.performed += context =>
            {
                HandleInput(LinButton.KeyButton.H);
            };
            InputController.Instance.controls.Interactions.Pazzle3.performed += context =>
            {
                HandleInput(LinButton.KeyButton.B);
            };
        }
    }

    private void HandleInput(LinButton.KeyButton key)
    {
        if (passed) return;

        if (!qteStarted)
        {
            if (key == queueKeys.Peek())
            {
                queueKeys.Dequeue();
                qteStarted = true;

                StartCoroutine(PassageButtons());
                IEnumerator PassageButtons()
                {
                    anims[0].SetBool("Passage", true);

                    for (int i = 1; i < anims.Length; i++)
                    {
                        yield return new WaitForSeconds(0.6f / buttons[i].timePassage);
                        anims[i].SetBool("Passage", true);
                    }
                }
            }
            return;
        }

        if (key == queueKeys.Peek())
        {
            queueKeys.Dequeue();
            anims[anims.Length - queueKeys.Count - 1].SetTrigger("Pressed");

            if (queueKeys.Count == 0)
            {
                GetComponent<Animator>().SetTrigger("Hide");
                GetComponent<PazzleController>().InvokePassedAction();
                passed = true;
            }
        }
        else
        {
            // Fail!
            Debug.Log("Fail");
            // GetComponent<Animator>().SetTrigger("Hide");
            // GetComponent<PazzleController>().InvokeFailedAction();
            passed = true;
        }
    }
}


// public class LinearButtons : MonoBehaviour
// {
//     public Animator[] buttonsYHB;
//     [SerializeField] private float speedPress;

//     private enum StartButton { Y, H, B }
//     [SerializeField] private StartButton startButton;

//     private bool saveY;
//     private bool saveH;
//     private bool saveB;

//     private bool passed;

//     private void Awake()
//     {
//         buttonsYHB[0].SetFloat("Speed", speedPress);
//         buttonsYHB[1].SetFloat("Speed", speedPress);
//         buttonsYHB[2].SetFloat("Speed", speedPress);

//         if (InputController.Instance != null)
//         {
//             // InputController.Instance.controls.Interactions.Pazzle1.performed += context =>
//             // {
//             //     if (startY) return;
//             //     buttonsYHB[0].SetBool("Press", true);
//             //     startY = true;
//             // };
//             // InputController.Instance.controls.Interactions.Pazzle2.performed += context =>
//             // {
//             //     if (failH) return;
//             //     if (buttonsYHB[1].GetComponent<Image>().fillAmount == 0.99f || buttonsYHB[1].GetBool("Press") == false || !startY || saveH)
//             //     {
//             //         failH = true;
//             //         return;
//             //     }
//             //     saveH = true;
//             // };
//             // InputController.Instance.controls.Interactions.Pazzle3.performed += context =>
//             // {
//             //     if (failB) return;
//             //     if (buttonsYHB[2].GetComponent<Image>().fillAmount == 0.99f || buttonsYHB[2].GetBool("Press") == false || !saveH || saveB)
//             //     {
//             //         failB = true;
//             //         return;
//             //     }
//             //     saveB = true;
//             // };
//         }
//     }

//     private void Update()
//     {
//         if (buttonsYHB[0].GetComponent<Image>().fillAmount == 0.99f)
//         {
//             buttonsYHB[1].SetBool("Press", true);
//         }
//         if (buttonsYHB[1].GetComponent<Image>().fillAmount == 0.99f)
//         {
//             buttonsYHB[2].SetBool("Press", true);
//         }

//         if (!passed && buttonsYHB[2].GetComponent<Image>().fillAmount == 0.99f)
//         {
//             else
//             {
//                 if (!saveY) buttonsYHB[0].SetBool("Unpress", true);
//                 if (!saveH) buttonsYHB[1].SetBool("Unpress", true);
//                 if (!saveB) buttonsYHB[2].SetBool("Unpress", true);
//             }
//         }

//         if (!passed && (buttonsYHB[0].GetComponent<Image>().fillAmount == 0.98f || buttonsYHB[1].GetComponent<Image>().fillAmount == 0.98f || buttonsYHB[2].GetComponent<Image>().fillAmount == 0.98f))
//         {
//         }
//     }
// }