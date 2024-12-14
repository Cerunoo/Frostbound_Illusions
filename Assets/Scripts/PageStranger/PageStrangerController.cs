using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Burst.CompilerServices;

public class PageStrangerController : MonoBehaviour
{
    [SerializeField] private float switchDelay = 0.5f;
    private bool isSwitchDelay;
    private Coroutine startSwitchDelay;

    [SerializeField] private float turnDelay = 0.5f;
    private bool isTurnDelay;
    private Coroutine startTurnDelay;

    [Space(5)]
    private Animator panelAnim;
    [SerializeField] private Animator pageAnim;
    [SerializeField] private Animator iconAnim;

    [Space(5), Header("Debug")]
    [SerializeField] private bool unlockPage;

    private void Start()
    {
        if (InputController.Instance != null) InputController.Instance.controls.PageStranger.SwitchOpen.performed += SwitchOpen;
        panelAnim = GetComponent<Animator>();

        unlockPage = false; // В будущем возможное считывание с PlayerPrefs
        if (unlockPage == false)
        {
            InputController.DisableInput(InputController.Instance.controls.PageStranger.SwitchOpen);
        }
        else
        {
            UnlockPage();
        }

        InputController.Instance.controls.PageStranger.TurnPaper.performed += context => {
            if (unlockPage && pageAnim.GetBool("Open") == true)
            {
                if (!isTurnDelay)
                {
                    pageAnim.SetTrigger("Turn");

                    if (startTurnDelay != null) StopCoroutine(startTurnDelay);
                    startTurnDelay = StartCoroutine(StartTurnDelay());
                }
            }

            IEnumerator StartTurnDelay()
            {
                isTurnDelay = true;

                yield return new WaitForSeconds(turnDelay);
                isTurnDelay = false;
            }
        };
        InputController.DisableInput(InputController.Instance.controls.PageStranger.TurnPaper);
    }

    // ////////////////////////////////////////////
    bool быложе = false;
    private void Update()
    {
        if (быложе) return;
        if (unlockPage)
        {
            быложе = true;
            UnlockPage();
        }
    }
    //////////////////////////////////////////////////

    private void SwitchOpen(InputAction.CallbackContext context)
    {
        if (!isSwitchDelay)
        {
            if (startSwitchDelay != null) StopCoroutine(startSwitchDelay);
            startSwitchDelay = StartCoroutine(StartSwitchDelay());

            bool open = !pageAnim.GetBool("Open");
            panelAnim.SetBool("Show", open);
            pageAnim.SetBool("Open", open);
            TimelineController.StaticDisableInteractions(open);
            if (open) InputController.EnableInput(InputController.Instance.controls.PageStranger.SwitchOpen);

            if (open) InputController.EnableInput(InputController.Instance.controls.PageStranger.TurnPaper);
            else if (!open) InputController.DisableInput(InputController.Instance.controls.PageStranger.TurnPaper);

            iconAnim.SetBool("Show", !open);
        }

        IEnumerator StartSwitchDelay()
        {
            isSwitchDelay = true;

            yield return new WaitForSeconds(switchDelay);
            isSwitchDelay = false;
        }
    }

    public void UnlockPage()
    {
        unlockPage = true;
        // В будущем возможная запись в PlayerPrefs

        iconAnim.SetBool("Show", true);
        iconAnim.SetTrigger("Updated");
        InputController.EnableInput(InputController.Instance.controls.PageStranger.SwitchOpen);
    }
}
