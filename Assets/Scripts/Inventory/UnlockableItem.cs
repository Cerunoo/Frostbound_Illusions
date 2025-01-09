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

    [Header("Debug")]
    [SerializeField] private bool withoutUnlock;

    private void Start()
    {
        if (Inventory.Instance == null) return; 
        inventory = Inventory.Instance;
    }

    private void TryReceiveItem()
    {
        if (!inventory.CanPickupItem())
        {
            NoticeHintController.Instance.ShowMessage("Недостаточно места..");
            button.pressed = false;
            return;
        }
        if (!inventory.CheckItem(requiredItemToUnlock))
        {
            NoticeHintController.Instance.ShowMessage("Необходим топор..");
            button.pressed = false;
            return;
        }

        onlyPazzle = Instantiate(pazzlePrefab, Vector2.zero, Quaternion.identity, parentSpawn);
        onlyPazzle.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        onlyPazzle.GetComponent<PazzleController>().passed += ReceiveItem;
        onlyPazzle.GetComponent<PazzleController>().failed += FailedReceive;

        onlyPazzle.GetComponent<LinearButtons>().AwakeManual(puzzleButtons, withoutUnlock);
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