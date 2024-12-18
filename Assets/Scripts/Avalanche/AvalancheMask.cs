using UnityEngine;
using System.Collections;

public class AvalancheMask : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedIncrease;
    [SerializeField] private float durationIncrease;

    private float startSizeX;

    private void Start()
    {
        startSizeX = transform.localScale.x;
        StartCoroutine(IncreaseMask());
    }

    private IEnumerator IncreaseMask()
    {
        float elapsedTime = 0;
        transform.localScale = new Vector2(0, transform.localScale.y);

        while(transform.localScale.x < startSizeX)
        {
            elapsedTime += Time.deltaTime;

            float x = transform.localScale.x;
            x = Mathf.Lerp(x, startSizeX, speedIncrease.Evaluate(elapsedTime / durationIncrease) / durationIncrease * Time.deltaTime);

            transform.localScale = new Vector2(x, transform.localScale.y);
            yield return null;
        }
    }
}
