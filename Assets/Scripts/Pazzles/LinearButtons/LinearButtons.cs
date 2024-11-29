using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LinearButtons : MonoBehaviour
{
    private LinButton[] buttons;

    [Space(5)]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform instParent;
    [SerializeField] private Transform[] posPoints;
    [SerializeField] private Sprite[] buttonSpritesYHB;

    private Queue<LinButton.KeyButton> queueKeys;
    private Animator[] anims;
    private bool qteStarted;

    private bool passed;

    public void AwakeManual(LinButton[] buttonsInput)
    {
        buttons = buttonsInput;

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
            anims[i].SetFloat("MultiPassage", buttons[i].speedPassage);

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
                    anims[0].SetTrigger("Pressed");

                    for (int i = 1; i < anims.Length; i++)
                    {
                        yield return new WaitForSeconds(buttons[i].waitPassage / buttons[i].speedPassage);
                        anims[i].SetBool("Passage", true);

                        if (i == anims.Length - 1)
                        {
                            yield return new WaitForSeconds(1 / buttons[i].speedPassage); // Где 1 - это время анимации Passage
                            if (queueKeys.Count != 0)
                            {
                                FinishPazzle(false);
                            }
                        }
                    }
                }
            }
            return;
        }

        if (key == queueKeys.Peek() && anims[anims.Length - queueKeys.Count].GetBool("Passage") == true)
        {
            queueKeys.Dequeue();
            anims[anims.Length - queueKeys.Count - 1].SetTrigger("Pressed");

            if (queueKeys.Count == 0)
            {
                FinishPazzle(true);
            }
        }
        else
        {
            FinishPazzle(false);
        }
    }

    private void FinishPazzle(bool pass)
    {
        if (passed) return;
        passed = true;

        if (pass)
        {
            GetComponent<PazzleController>().InvokePassedAction();
            GetComponent<Animator>().SetTrigger("Hide");
        }
        else
        {
            StartCoroutine(UnPassageButtons());
            IEnumerator UnPassageButtons()
            {
                for (int i = anims.Length - 1; i >= anims.Length - queueKeys.Count - 1; i--)
                {
                    anims[i].SetTrigger("UnPassage");
                }

                yield return new WaitForSeconds(1f);
                GetComponent<PazzleController>().InvokeFailedAction();
                GetComponent<Animator>().SetTrigger("Hide");
            }
        }
    }
}