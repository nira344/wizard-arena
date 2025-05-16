using UnityEngine;

public class bossBar : MonoBehaviour
{

    void Start()
    {
        Hide();
    }

    public void Show()
    {
        CanvasRenderer[] childRenderers = GetComponentsInChildren<CanvasRenderer>();

        foreach (CanvasRenderer renderer in childRenderers)
        {
            // Make sure we're not disabling the renderer on the parent (if it somehow has one)
            if (renderer.gameObject != gameObject)
            {
                renderer.SetAlpha(255);
            }
        }

        Debug.Log("boss bar enabled");
    }

    public void Hide()
    {
        CanvasRenderer[] childRenderers = GetComponentsInChildren<CanvasRenderer>();

        foreach (CanvasRenderer renderer in childRenderers)
        {
            // Make sure we're not disabling the renderer on the parent (if it somehow has one)
            if (renderer.gameObject != gameObject)
            {
                renderer.SetAlpha(0);
            }
        }

        Debug.Log("boss bar disabled");
    }
}
