using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private int indexForestScene;

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexForestScene);
    }
}
