using UnityEngine;

public class Forest2Manager : MonoBehaviour
{
    [SerializeField] private Animator verrden;

    private void Start()
    {
        GetBP();
    }

    private void GetBP()
    {
        if (Inventory.Instance != null) Inventory.Instance.GetComponent<Animator>().SetBool("hide", false);
        InputController.EnableInput(InputController.Instance.controls.Inventory.SwitchOpen);
        verrden.SetBool("GetBP", true);
    }
}