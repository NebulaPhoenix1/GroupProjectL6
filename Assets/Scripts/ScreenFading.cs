using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class ScreenFading : MonoBehaviour
{
    public UnityEvent onFadeComplete;

    [SerializeField] private int targetAlpha; //Final alpha value at the end of fade
    [SerializeField] private float startingAlpha; //Initial alpha value at the start of fade
    [SerializeField] private float duration; //How long the fade will take
    private Image image; //The image that's having its alpha changed
    private Color imageColor;

    void Start()
    {
        image = GetComponent<Image>();
        
    }

    public void StartFade()
    {
        imageColor = image.color;
        imageColor.a = startingAlpha;
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate how far along we are
            float lerpValue = elapsedTime / duration;

            // Move alpha from start to target based on time
            imageColor.a = Mathf.Lerp(startingAlpha, targetAlpha, lerpValue);
            image.color = imageColor;

            yield return null;
        }

        // Ensure we hit the exact target alpha at the end
        imageColor.a = targetAlpha;
        image.color = imageColor;
        onFadeComplete.Invoke();
    }
}