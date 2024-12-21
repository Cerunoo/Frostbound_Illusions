using UnityEngine;

public class RunPlatform : MonoBehaviour
{
    private bool runPastValue;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            runPastValue = PlayerController.Instance.isRunning;
            PlayerController.Instance.isRunning = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.Instance.isRunning = runPastValue;
        }
    }
}
