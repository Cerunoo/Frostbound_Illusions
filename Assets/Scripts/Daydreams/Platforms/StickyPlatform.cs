using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.isSticky = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance != null) PlayerController.Instance.isSticky = false;
        }
    }
}
