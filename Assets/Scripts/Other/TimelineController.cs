using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController Instance { get; private set; }

    public bool isPlay { get; private set; }
    
    private PlayableDirector director;

    private void Awake()
    {
        Instance = this;

        director = GetComponent<PlayableDirector>();
        director.played += OnPlayed;
        director.stopped += OnStopped;
        if (director.playOnAwake) OnPlayed();
    }

    private void OnPlayed(PlayableDirector director = null)
    {
        DisableInteractions(true);
        isPlay = true;
    }

    private void OnStopped(PlayableDirector director)
    {
        DisableInteractions(false);
        isPlay = false;
    }

    public void PlayAsset(PlayableAsset asset)
    {
        director.Play(asset);
    }

    public void PlayAsset(PlayableAsset asset, DirectorWrapMode mode)
    {
        director.Play(asset, mode);
    }

    private bool inventoryStateWorkPastValue;
    private void DisableInteractions(bool disable)
    {
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = disable;
        if (Inventory.Instance != null)
        {
            if (disable)
            {
                inventoryStateWorkPastValue = Inventory.Instance.GetStateWork();
                Inventory.Instance.SetStateWork(!disable);
            }
            else
            {
                Inventory.Instance.SetStateWork(inventoryStateWorkPastValue);
            }
        }

        InteractionButton[] buttons = FindObjectsByType<InteractionButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (disable) Array.ForEach(buttons, InteractionButton.StaticDisable);
        else if (!disable) Array.ForEach(buttons, InteractionButton.StaticEnable);

        if (disable)
        {
            InputController.DisableAllInput();
            InputController.EnableInput(InputController.Instance.controls.Player.Horizontal);
        }
        if (!disable) InputController.EnableAllInput();
    }

    public static void StaticDisableInteractions(bool disable)
    {
        if (Instance != null) Instance.DisableInteractions(disable);
    }
}
