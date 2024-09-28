using UnityEngine;

public class DeliveryQuest : Quest
{
    [SerializeField] private GameObject[] desiredObjects;

    protected override bool CheckQuest() // Рекурсией попробуй сделать
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        for (int i = 0; i < desiredObjects.Length; i++)
        {
            for (int j = 0; j < inventory.slotItems.Length; j++)
            {
                if (desiredObjects[i] == inventory.slotItems[j])
                {
                    inventory.DropItem(true, j);
                    break;
                }
                else if (j == inventory.slotItems.Length - 1) return false;
            }
        }
        return true;
    }
}