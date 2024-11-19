using System;
using UnityEngine;

public class PazzleController : MonoBehaviour
{
    public event Action passed;
    public event Action failed;

    private PlayerController player;

    private void Start()
    {
        if (PlayerController.Instance != null) player = PlayerController.Instance;
        player.disableMove = true;
    }

    public void InvokePassedAction()
    {
        passed?.Invoke();
        player.disableMove = false;
    }

    public void InvokeFailedAction()
    {
        failed?.Invoke();
        player.disableMove = false;
    }
}