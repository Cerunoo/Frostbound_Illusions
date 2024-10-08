using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if ((bool)Inventory.Instance?.TryPickupItem(GetComponent<SpriteRenderer>().sprite, itemName))
            {
                Destroy(gameObject);
            }
        }
    }
}