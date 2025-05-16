using UnityEngine;
using TMPro; // Or use TMPro if using TextMeshPro

public class TextSizeOscillator : MonoBehaviour
{
    public float minFontSize = 20f;
    public float maxFontSize = 40f;
    public float speed = 1f;

    // Support for both Text and TextMeshProUGUI
    private TextMeshProUGUI tmpText;

    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();

        if (tmpText == null)
        {
            Debug.LogWarning("No Text or TextMeshProUGUI component found on this GameObject.");
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float fontSize = Mathf.Lerp(minFontSize, maxFontSize, t);
        
        if (tmpText != null)
        {
            tmpText.fontSize = fontSize; // TMP uses float font size
        }
    }
}