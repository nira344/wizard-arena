using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class textUtilities : MonoBehaviour
{
    private TextMeshProUGUI gurt;
    private float timer;
    private bool fadeOutQueued = false;
    private float fadeOutTime;

    void Start()
    {
        gurt = GetComponent<TextMeshProUGUI>();
        if (!gurt) {Debug.LogWarning("GURT HAS ESCAPED CONTAINMENT");}
        fadeOutQueued = false;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (fadeOutQueued && timer <= 0)
        {
            timer = 0;
            fadeOutQueued = false;
            FadeOut(fadeOutTime);
        }
    }

    public void TitleAppear(string title, float fadeIn, float waitTime, float fadeOut)
    {
        // Setup
        timer = waitTime;
        SetAlpha(0);
        gurt.text = title;

        // Low-Taper Fade
        FadeIn(fadeIn);
        fadeOutTime = fadeOut;
        fadeOutQueued = true;
    }

    public void FadeIn(float seconds)
    {
        SetAlpha(0f); // Start fully transparent
        StartCoroutine(FadeInCoroutine(seconds));
    }

    public void FadeOut(float seconds)
    {
        SetAlpha(0f); // Start fully opaque
        StartCoroutine(FadeOutCoroutine(seconds));
    }

    private System.Collections.IEnumerator FadeInCoroutine(float seconds)
    {
        float elapsedTime = 0f;
        Color originalColor = gurt.color;

        while (elapsedTime < seconds)
        {
            float alpha = Mathf.Clamp01(elapsedTime / seconds);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f); // Ensure it's fully visible at the end
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float seconds)
    {
        float elapsedTime = 0f;
        Color originalColor = gurt.color;

        while (elapsedTime < seconds)
        {
            float alpha = Mathf.Clamp01(elapsedTime / seconds);
            SetAlpha(255 - alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f); // Ensure it's fully visible at the end
    }

    private void SetAlpha(float alpha)
    {
        Color colorized = gurt.color;
        colorized.a = alpha;
        gurt.color = colorized;
    }
}
