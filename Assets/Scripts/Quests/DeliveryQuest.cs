using UnityEngine;

public class DeliveryQuest : Quest
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] desiredObjects;

    protected override bool CheckQuest()
    {
        bool result = true;

        for (int i = 0; i < desiredObjects.Length; i++)
        {
            if (desiredObjects[i] == null) continue;

            for (int j = 0; j < inventory.slotItems.Length; j++)
            {
                if (desiredObjects[i] == inventory.slotItems[j])
                {
                    desiredObjects[i] = null;
                    inventory.DropItem(true, j);
                    break;
                }
                else if (j == inventory.slotItems.Length - 1) result = false;
            }
        }

        return result;
    }
}