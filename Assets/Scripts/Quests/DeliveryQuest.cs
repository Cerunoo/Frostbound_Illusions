using UnityEngine;

public class DeliveryQuest : Quest
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject[] desiredObjects;

    // protected override bool CheckQuest()
    // {
    //     return CheckQuestRecursive();
    // }

    // private bool CheckQuestRecursive(int i, int j)
    // {

    // }

    protected override bool CheckQuest() // Рекурсией попробуй сделать
    {
        for (int i = 0; i < desiredObjects.Length; i++)
        {
            for (int j = 0; j < inventory.slotItems.Length; j++)
            {
                if (desiredObjects[i] == inventory.slotItems[j])
                {
                    desiredObjects[i] = null;
                    inventory.DropItem(true, j);
                    break;
                }
                else if (j == inventory.slotItems.Length - 1) return false;
            }
        }
        return true;
    }
}