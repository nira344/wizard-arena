using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public string sceneToLoad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneToLoad != null)
        {
            if (Input.GetKeyDown("f"))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
