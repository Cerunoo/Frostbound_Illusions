using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundQueue : MonoBehaviour
{
    [SerializeField] private SoundObject[] sounds;
    private Queue<SoundObject> queue;

    [Header("Debug"), Space(3)]
    [SerializeField] private bool skipSound;
    [SerializeField] private bool skipLoopSound;
    private SoundObject onlySound;
    private Coroutine onlyWaitCoroutine;
    private Coroutine onlyPlayCoroutine;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        queue = new Queue<SoundObject>(sounds);

        PlayNextSound();
    }

    private void Update()
    {
        if (skipSound)
        {
            PlayNextSound();
            skipSound = false;
        }
        if (skipLoopSound)
        {
            PlayNextSound(true);
            skipLoopSound = false;
        }
    }

    private IEnumerator WaitNextSound()
    {
        yield return new WaitForSeconds(source.GetClipRemainingTime());
        PlayNextSound();
    }

    public void PlayNextSound(bool killLoop = false)
    {
        if (onlyPlayCoroutine != null) StopCoroutine(onlyPlayCoroutine);
        onlyPlayCoroutine = StartCoroutine(PlayNextSoundCoroutine(killLoop));
    }

    private IEnumerator PlayNextSoundCoroutine(bool killLoop)
    {
        if (queue.Count == 0) yield break;

        float fadeD;
        if (onlySound != null) fadeD = onlySound.fadeDuration;
        else fadeD = 0;
        if (fadeD != 0)
        {
            while (source.volume != 0)
            {
                source.volume = Mathf.Max(0, source.volume - (Time.deltaTime / fadeD));
                yield return null;
            }
        }

        onlySound = queue.Dequeue();
        if (killLoop) onlySound = queue.Dequeue();
        source.clip = onlySound.clip;
        if (onlySound.delay == 0) source.Play();
        else source.PlayDelayed(onlySound.delay);
        if (onlySound.loop)
        {
            Queue<SoundObject> loopQueue = new Queue<SoundObject>();
            onlySound.delay = onlySound.loopDelay;
            loopQueue.Enqueue(onlySound);

            queue = new Queue<SoundObject>(loopQueue.Concat(queue));
        }

        if (fadeD != 0)
        {
            while (source.volume != 1)
            {
                source.volume = Mathf.Min(1, source.volume + (Time.deltaTime / fadeD));
                yield return null;
            }
        }

        if (onlyWaitCoroutine != null) StopCoroutine(onlyWaitCoroutine);
        onlyWaitCoroutine = StartCoroutine(WaitNextSound());
    }
}