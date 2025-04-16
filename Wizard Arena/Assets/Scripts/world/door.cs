using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public string sceneToLoad;
    private GameObject player;
    private bool activated = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneToLoad != null)
        {
            if (Input.GetKeyDown("f") && TouchingPlayer() && activated)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private bool TouchingPlayer()
    {
        return gameObject.GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>());
    }
}
