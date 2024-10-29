using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public GameObject[] slots;
    public bool[] isFull;
    public GameObject[] slotItems;

    Animator anim;
    bool animIsOn;
    bool isOpen;
    bool isWork = true;

    public Color normalColor;
    public Color selectedColor;
    int selectedSlot = 0;

    private PlayerController player;
    public Vector2 spawnDistance;

    private void Awake()
    {
        Instance = this;

        anim = gameObject.GetComponent<Animator>();
        if (PlayerController.Instance != null) player = PlayerController.Instance;

        if (InputController.Instance != null)
        {
            InputController.Instance.controls.Inventory.SwitchOpen.performed += context =>
            {
                if (!animIsOn && isWork) SwitchInventory();
            };

            InputController.Instance.controls.Inventory.SelectLeft.performed += context => SelectLeft();
            InputController.Instance.controls.Inventory.SelectRight.performed += context => SelectRight();

            InputController.Instance.controls.Inventory.DropItem.performed += context => DropItem();
        }
    }

    private void SelectLeft()
    {
        if (isOpen && !animIsOn && isWork)
        {
            slots[selectedSlot].GetComponent<Image>().color = normalColor;
            if (selectedSlot != 0)
            {
                selectedSlot--;
            }
            else
            {
                selectedSlot = slots.Length - 1;
            }
            slots[selectedSlot].GetComponent<Image>().color = selectedColor;
        }
    }
    private void SelectRight()
    {
        if (isOpen && !animIsOn && isWork)
        {
            slots[selectedSlot].GetComponent<Image>().color = normalColor;
            if (selectedSlot != slots.Length - 1)
            {
                selectedSlot++;
            }
            else
            {
                selectedSlot = 0;
            }
            slots[selectedSlot].GetComponent<Image>().color = selectedColor;
        }
    }

    public void DropItem(bool forciblyRemove = false, int indexSlot = 4)
    {
        if ((isOpen && !animIsOn && isWork) || forciblyRemove)
        {
            int slot = !forciblyRemove ? selectedSlot : indexSlot;

            if (!isFull[slot]) return;
            isFull[slot] = false;

            Image slotImg = slots[slot].transform.GetChild(0).GetComponent<Image>();
            slotImg.gameObject.SetActive(false);

            if (!forciblyRemove)
            {
                Vector2 spawnPos = new Vector2(player.transform.position.x + spawnDistance.x * (player.facingRight ? 1 : -1), player.transform.position.y + spawnDistance.y);
                if (!forciblyRemove) Instantiate(slotItems[slot], spawnPos, Quaternion.identity);
            }
            slotItems[slot] = null;
        }
    }

    public void PickupItem(Sprite itemSprite, string itemName)
    {
        for (int i = 0; i < isFull.Length; i++)
        {
            if (isFull[i] == false)
            {
                Image slotImage = slots[i].transform.GetChild(0).GetComponent<Image>();
                slotImage.sprite = itemSprite;
                slotImage.gameObject.SetActive(true);

                slotItems[i] = Resources.Load(itemName) as GameObject;
                isFull[i] = true;
                return;
            }
        }
    }

    public bool CanPickupItem()
    {
        for (int i = 0; i < isFull.Length; i++)
        {
            if (isFull[i] == false) return true;
        }
        return false;
    }

    private void SwitchInventory()
    {
        anim.SetBool("open", !isOpen);
        animIsOn = true;

        if (!isOpen)
        {
            selectedSlot = 0;
            slots[selectedSlot].GetComponent<Image>().color = selectedColor;
        }

        StartCoroutine(waitEnd());
        IEnumerator waitEnd()
        {
            // Задержка перед возможностью следующего вызова SwitchInventory
            yield return new WaitForSeconds(0.5f);
            animIsOn = false;
            isOpen = !isOpen;

            if (isOpen == false)
            {
                slots[selectedSlot].GetComponent<Image>().color = normalColor;
            }
        }
    }

    public void SwitchStateWork()
    {
        isWork = !isWork;

        if (isWork == false)
        {
            if (isOpen || animIsOn)
            {
                anim.SetBool("open", false);
                animIsOn = true;

                slots[selectedSlot].GetComponent<Image>().color = normalColor;

                StartCoroutine(waitEnd());
                IEnumerator waitEnd()
                {
                    yield return new WaitForSeconds(0.5f);
                    animIsOn = false;
                    isOpen = false;

                    // anim.SetBool("hide", true);
                }
            }
            else
            {
                // anim.SetBool("hide", true);
            }
        }
        else
        {
            // anim.SetBool("hide", false);
        }
    }
}