using UnityEngine;

public class UnlockableItem : MonoBehaviour
{
    [SerializeField] private Item receivedItem;

    private void ReceiveItem()
    {
        if ((bool)Inventory.Instance?.TryPickupItem(receivedItem.GetComponent<SpriteRenderer>().sprite, receivedItem.itemName))
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponentInChildren<InteractionButton>().pressed = false;
        }
    }

    private void OnEnable() => GetComponentInChildren<InteractionButton>().btnPress += ReceiveItem;
    private void OnDisable() => GetComponentInChildren<InteractionButton>().btnPress -= ReceiveItem;
}