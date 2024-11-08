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
    }

    private void GetBP(Quest quest)
    {
        if (Inventory.Instance != null) Inventory.Instance.GetComponent<Animator>().SetBool("hide", false);
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
        yield return new WaitForSeconds(2f);
        AsyncLoading.LoadScene(indexHouseScene);
    }
}