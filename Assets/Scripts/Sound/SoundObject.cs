using UnityEngine;

[System.Serializable]
public class SoundObject
{
    public AudioClip clip;
    [Space(5)]
    public float delay;
    public float fadeDuration;
    [Space(5)]
    public bool loop;
    public float loopDelay;
}