using UnityEngine;

public class UnStickyPlatform : MonoBehaviour
{
    [SerializeField] private bool pastStickyValue;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null)
            {
                pastStickyValue = PlayerController.Instance.isSticky;
                PlayerController.Instance.isSticky = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.isSticky = pastStickyValue;
        }
    }
}
