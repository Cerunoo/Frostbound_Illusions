using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    Image slotImg = inventory.slots[i].transform.GetChild(0).GetComponent<Image>();
                    slotImg.gameObject.SetActive(true);
                    slotImg.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;

                    inventory.isFull[i] = true;
                    inventory.slotItems[i] = Resources.Load(itemName) as GameObject;

                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}