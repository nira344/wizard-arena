using UnityEngine;

public class MainMenuSetup : MonoBehaviour
{
    void Start()
    {
        var persistentCam = GameObject.Find("MainSceneCamera");
        if (persistentCam != null)
        {
            Destroy(persistentCam);
            Debug.Log("Destroyed persistent camera from previous scene.");
        }
    }
}
