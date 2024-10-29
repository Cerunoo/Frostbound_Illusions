using UnityEngine;

public class UnlockableItem : MonoBehaviour
{
    [SerializeField] private Item receivedItem;
    public GameObject pazzlePrefab;

    private Inventory inventory;

    private void Start()
    {
        if (Inventory.Instance == null) return; 
        inventory = Inventory.Instance;
    }

    private void TryReceiveItem()
    {
        if (inventory.CanPickupItem())
        {
            GameObject pazzle = Instantiate(pazzlePrefab, Vector2.zero, Quaternion.identity, FindFirstObjectByType<Canvas>().transform);
            pazzle.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            pazzle.GetComponent<PazzleController>().passed += ReceiveItem;
        }
        else
        {
            GetComponentInChildren<InteractionButton>().pressed = false;
        }
    }

    private void ReceiveItem()
    {
        inventory.PickupItem(receivedItem.GetComponent<SpriteRenderer>().sprite, receivedItem.itemName);
        Destroy(gameObject);
    }

    private void OnEnable() => GetComponentInChildren<InteractionButton>().btnPress += TryReceiveItem;
}