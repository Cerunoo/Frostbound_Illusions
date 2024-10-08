using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    [SerializeField] private float dropTime;
    [SerializeField] private float destroyTime;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("DropPlatform", dropTime);
            Destroy(gameObject, destroyTime);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
