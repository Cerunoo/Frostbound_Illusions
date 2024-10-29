using System.Collections;
using UnityEngine;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private InteractionButton homeButton;
    [SerializeField] private int indexHouseScene;

    private void LoadHouseScene()
    {
        StartCoroutine(Wait());
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = true;
        if (Inventory.Instance != null) Inventory.Instance.SwitchStateWork();
    }

    private IEnumerator Wait() // Искусственное ожидание загрузки сцены, временно
    {
        yield return new WaitForSeconds(2f);
        AsyncLoading.LoadScene(indexHouseScene);
    }

    private void OnEnable() => homeButton.btnPress += LoadHouseScene;
}