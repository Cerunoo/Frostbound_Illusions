using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Inventory.Instance == null) return;
            Inventory inventory = Inventory.Instance;

            if (inventory.CanPickupItem())
            {
                inventory.PickupItem(GetComponent<SpriteRenderer>().sprite, itemName);
                Destroy(gameObject);
            }
        }
    }
}