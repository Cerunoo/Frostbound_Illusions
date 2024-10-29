using System;
using UnityEngine;

public class PazzleController : MonoBehaviour
{
    public event Action passed;

    private PlayerController player;
    private Inventory inventory;

    private void Start()
    {
        if (PlayerController.Instance != null) player = PlayerController.Instance;
        player.disableMove = true;

        if (Inventory.Instance != null) inventory = Inventory.Instance;
        inventory.SwitchStateWork();
    }

    public void InvokePassedAction()
    {
        passed?.Invoke();
        player.disableMove = false;
        inventory.SwitchStateWork();
    }
}