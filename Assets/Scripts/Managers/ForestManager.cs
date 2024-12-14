using System.Collections;
using UnityEngine;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private InteractionButton homeButton;
    [SerializeField] private int indexHouseScene;

    [SerializeField] private DeliveryQuest logQuest;

    private void OnEnable()
    {
        homeButton.btnPress += LoadHouseScene;
        logQuest.QuestGotEvent += GetBP;
        InputController.DisableInput(InputController.Instance.controls.Inventory.SwitchOpen);
    }

    private void GetBP(Quest quest)
    {
        if (Inventory.Instance != null) Inventory.Instance.GetComponent<Animator>().SetBool("hide", false);
        InputController.EnableInput(InputController.Instance.controls.Inventory.SwitchOpen);
        logQuest.GetComponent<Animator>().SetBool("GetBP", true);
    }

    private void LoadHouseScene()
    {
        StartCoroutine(Wait());
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = true;
        if (Inventory.Instance != null) Inventory.Instance.SetStateWork(false);
    }

    private IEnumerator Wait() // Искусственное ожидание загрузки сцены, временно
    {
        yield return new WaitForSecondsRealtime(2f);
        AsyncLoading.LoadScene(indexHouseScene);
    }
}