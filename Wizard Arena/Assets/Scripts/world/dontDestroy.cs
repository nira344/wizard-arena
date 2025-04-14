using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Keeps it between scenes
    }
}
