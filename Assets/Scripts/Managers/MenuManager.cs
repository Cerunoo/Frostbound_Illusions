using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private int indexForestScene;

    private void Start()
    {
        LoadScene(indexForestScene);
    }

    public void LoadScene(int indexScene)
    {
        StartCoroutine(Wait(indexScene));
    }

    private IEnumerator Wait(int index) // Искусственное ожидание загрузки сцены, временно
    {
        yield return new WaitForSecondsRealtime(3f);
        AsyncLoading.LoadScene(index);
    }
}
