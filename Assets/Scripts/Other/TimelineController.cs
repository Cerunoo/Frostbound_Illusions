using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController Instance { get; private set; }

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
    }

    private void OnStopped(PlayableDirector director)
    {
        DisableInteractions(false);
    }

    private void DisableInteractions(bool disable)
    {
        if (PlayerController.Instance != null) PlayerController.Instance.disableMove = disable;
        if (Inventory.Instance != null) Inventory.Instance.SetStateWork(!disable);

        InteractionButton[] buttons = FindObjectsByType<InteractionButton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (disable) Array.ForEach(buttons, InteractionButton.StaticDisable);
        else if (!disable) Array.ForEach(buttons, InteractionButton.StaticEnable);
    }

    public void PlayAsset(PlayableAsset asset, DirectorWrapMode mode = DirectorWrapMode.None)
    {
        director.Play(asset, mode);
    }
}
