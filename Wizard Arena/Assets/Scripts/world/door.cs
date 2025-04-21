using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public string sceneToLoad;
    public bool locked = false;
    public float spawnX;
    public float spawnY;

    private GameObject player;

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
            if (Input.GetKeyDown("f") && TouchingPlayer() && !locked)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private bool TouchingPlayer()
    {
        return gameObject.GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>());
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
}
