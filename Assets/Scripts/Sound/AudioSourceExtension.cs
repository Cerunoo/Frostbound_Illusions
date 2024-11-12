using UnityEngine;

public static class AudioSourceExtension
{
    public static bool IsReversePitch(this AudioSource source)
    {
        return source.pitch < 0f;
    }

    public static float GetClipRemainingTime(this AudioSource source)
    {
        float remainingTime = (source.clip.length - source.time) / source.pitch;
        return source.IsReversePitch() ? (source.clip.length + remainingTime) : remainingTime;
    }
}