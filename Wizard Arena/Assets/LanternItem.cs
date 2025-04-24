using UnityEngine;

public class LanternItem : MonoBehaviour, IUsableItem
{
    public void Use(GameObject user)
    {
        Debug.Log("Lantern used!");
        // Activate glow, etc.
    }
}