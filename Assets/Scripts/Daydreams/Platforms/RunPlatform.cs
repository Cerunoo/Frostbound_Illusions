using UnityEngine;

public class RunPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.isRunning = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.isRunning = false;
        }
    }
}
