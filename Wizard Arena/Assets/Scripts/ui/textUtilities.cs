using UnityEngine;
using TMPro;

public class textUtilities : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gurt; // Assign in inspector
    private float timer;
    private bool fadeOutQueued = false;
    private float fadeOutTime;

    void Start()
    {
        if (!gurt)
        {
            gurt = GetComponent<TextMeshProUGUI>();
            if (!gurt)
            {
                Debug.LogWarning("textUtilities: No TextMeshProUGUI found on this object.");
                return;
            }
        }
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
        if (gurt == null)
        {
            Debug.LogWarning("textUtilities: Cannot show title, gurt is null.");
            return;
        }

        timer = waitTime;
        SetAlpha(0);
        gurt.text = title;

        FadeIn(fadeIn);
        fadeOutTime = fadeOut;
        fadeOutQueued = true;
    }

    public void FadeIn(float seconds)
    {
        if (gurt == null) return;
        SetAlpha(0f); // Start fully transparent
        StartCoroutine(FadeInCoroutine(seconds));
    }

    public void FadeOut(float seconds)
    {
        if (gurt == null) return;
        StartCoroutine(FadeOutCoroutine(seconds));
    }

    private System.Collections.IEnumerator FadeInCoroutine(float seconds)
    {
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            float alpha = Mathf.Clamp01(elapsedTime / seconds);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
    }

    private System.Collections.IEnumerator FadeOutCoroutine(float seconds)
    {
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            float alpha = 1f - Mathf.Clamp01(elapsedTime / seconds);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (gurt == null)
        {
            Debug.LogWarning("textUtilities: Tried to set alpha but gurt is null.");
            return;
        }

        Color colorized = gurt.color;
        colorized.a = alpha;
        gurt.color = colorized;
    }
}
