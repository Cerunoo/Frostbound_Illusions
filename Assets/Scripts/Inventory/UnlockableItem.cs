using UnityEngine;

public class UnlockableItem : MonoBehaviour
{
    [SerializeField] private Item receivedItem;
    [SerializeField] private InteractionButton button;
    public GameObject pazzlePrefab;
    [SerializeField] private Transform parentSpawn;

    [SerializeField] private GameObject requiredItemToUnlock;

    [SerializeField] private LinButton[] puzzleButtons;

    private Inventory inventory;
    private GameObject onlyPazzle;

    private void Start()
    {
        if (Inventory.Instance == null) return; 
        inventory = Inventory.Instance;
    }

    private void TryReceiveItem()
    {
        if (inventory.CanPickupItem() && inventory.CheckItem(requiredItemToUnlock))
        {
            onlyPazzle = Instantiate(pazzlePrefab, Vector2.zero, Quaternion.identity, parentSpawn);
            onlyPazzle.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            onlyPazzle.GetComponent<PazzleController>().passed += ReceiveItem;
            onlyPazzle.GetComponent<PazzleController>().failed += FailedReceive;

            onlyPazzle.GetComponent<LinearButtons>().AwakeManual(puzzleButtons);
        }
        else
        {
            button.pressed = false;
        }
    }

    private void ReceiveItem()
    {
        if (onlyPazzle != null)
        {
            onlyPazzle.GetComponent<PazzleController>().passed -= ReceiveItem;
            onlyPazzle.GetComponent<PazzleController>().failed -= FailedReceive;
        }

        inventory.PickupItem(receivedItem.GetComponent<SpriteRenderer>().sprite, receivedItem.itemName);
        Destroy(gameObject);
    }

    private void FailedReceive()
    {
        button.pressed = false;

        if (onlyPazzle != null)
        {
            onlyPazzle.GetComponent<PazzleController>().passed -= ReceiveItem;
            onlyPazzle.GetComponent<PazzleController>().failed -= FailedReceive;
        }
    }

    private void OnEnable() => button.btnPress += TryReceiveItem;
}