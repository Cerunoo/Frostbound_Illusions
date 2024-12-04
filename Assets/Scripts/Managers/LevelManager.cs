using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Animator optionsAnim;

    private void Awake()
    {
        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Leveled.Escape.performed += context =>
            {
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
                optionsAnim.SetBool("Show", !optionsAnim.GetBool("Show"));
            };
        }
    }

    public void LoadScene(int indexScene)
    {
        StartCoroutine(Wait(indexScene));
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = true;
        if (Inventory.Instance != null) Inventory.Instance.SetStateWork(false);
    }

    private IEnumerator Wait(int index) // Искусственное ожидание загрузки сцены, временно
    {
        yield return new WaitForSecondsRealtime(2f);
        AsyncLoading.LoadScene(index);
        Time.timeScale = 1;
    }
}