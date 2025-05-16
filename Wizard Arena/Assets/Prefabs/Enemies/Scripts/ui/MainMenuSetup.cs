using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetup : MonoBehaviour
{
    private Scene thisScene;

    void Awake()
    {
        // Store the scene this GameObject belongs to
        thisScene = gameObject.scene;
        Camera.main.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene loadedScene, LoadSceneMode mode)
    {
        // Log only if the scene that loaded is the one this GameObject is in
        if (loadedScene == thisScene)
        {
            Camera.main.gameObject.SetActive(true);
        }
    }
}
