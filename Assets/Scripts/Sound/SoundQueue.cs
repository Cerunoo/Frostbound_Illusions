using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundQueue : MonoBehaviour
{
    [SerializeField] private SoundObject[] sounds;
    private Queue<SoundObject> queue;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        queue = new Queue<SoundObject>(sounds);

        PlayNextSound();
        StartCoroutine(WaitNextSound());
    }

    private IEnumerator WaitNextSound()
    {
        var waitForClipRemainingTime = new WaitForSeconds(source.GetClipRemainingTime());
        yield return waitForClipRemainingTime;

        PlayNextSound();
        StartCoroutine(WaitNextSound());
    }

    public void PlayNextSound(bool killLoop = false)
    {
        SoundObject sound = queue.Dequeue();
        if (killLoop) sound = queue.Dequeue();
        source.clip = sound.clip;
        if (sound.delay == 0) source.Play();
        else source.PlayDelayed(sound.delay);

        if (sound.loop)
        {
            Queue<SoundObject> loopQueue = new Queue<SoundObject>();
            sound.delay = sound.loopDelay;
            loopQueue.Enqueue(sound);

            queue = new Queue<SoundObject>(loopQueue.Concat(queue));
        }
    }
}