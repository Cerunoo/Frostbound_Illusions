using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FollusionController;

public class Inventory : MonoBehaviour
{
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

    [SerializeField] private InputController input;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();

        input.controls.Inventory.SwitchOpen.performed += context => { if (!animIsOn && isWork) SwitchInventory(); };

        input.controls.Inventory.SelectLeft.performed += context =>
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
        };
        input.controls.Inventory.SelectRight.performed += context =>
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
        };

        input.controls.Inventory.DropItem.performed += context =>
        {
            if (isOpen && !animIsOn && isWork)
            {
                if (!isFull[selectedSlot]) return;
                isFull[selectedSlot] = false;

                Image slotImg = slots[selectedSlot].transform.GetChild(0).GetComponent<Image>();
                slotImg.gameObject.SetActive(false);

                Vector2 spawnPos = new Vector2(player.transform.position.x + spawnDistance.x * (player.facingRight ? 1 : -1), player.transform.position.y + spawnDistance.y);
                Instantiate(slotItems[selectedSlot], spawnPos, Quaternion.identity);
                slotItems[selectedSlot] = null;
            }
        };
    }

    void SwitchInventory()
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

                    anim.SetBool("hide", true);
                }
            }
            else
            {
                anim.SetBool("hide", true);
            }
        }
        else
        {
            anim.SetBool("hide", false);
        }
    }
}