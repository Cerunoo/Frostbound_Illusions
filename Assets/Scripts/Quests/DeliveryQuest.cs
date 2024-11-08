using UnityEngine;

public class DeliveryQuest : Quest
{
    [Space(15)]
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] desiredObjects;

    [SerializeField] private bool destroyFindItemInInventory;

    protected override bool CheckQuest()
    {
        if (destroyFindItemInInventory)
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
        else
        {
            bool[] foundItems = new bool[inventory.slotItems.Length];

            for (int i = 0; i < desiredObjects.Length; i++)
            {
                for (int j = 0; j < inventory.slotItems.Length; j++)
                {
                    if (desiredObjects[i] == inventory.slotItems[j] && foundItems[j] != true)
                    {
                        foundItems[j] = true;
                        break;
                    }
                    else if (j == inventory.slotItems.Length - 1) return false;
                }
            }
            return true;
        }
    }
}