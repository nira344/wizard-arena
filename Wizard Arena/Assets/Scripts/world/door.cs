using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public GameObject exitDoor;
    public bool locked = false;

    private GameObject player;
    private BoxCollider2D box;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TouchingPlayer())
        {
            if (Input.GetKeyDown("f") && !locked)
            {
                player.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y, player.transform.position.z);
            }
        }
    }

    bool TouchingPlayer()
    {
        Collider2D hit = Physics2D.OverlapBox(box.bounds.center, box.bounds.size, 0f);
        return (bool)(hit.gameObject == player);
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
