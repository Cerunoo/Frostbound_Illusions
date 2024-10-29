using UnityEngine;
// using UnityEngine.SceneManagement;

public class DaydreamsManager : MonoBehaviour
{
    private void Update()
    {
        if (PlayerController.Instance?.FrameDirection.y <= -35f)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}