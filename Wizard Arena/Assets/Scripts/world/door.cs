using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public GameObject exitDoor;
    public bool locked = false;
    public Vector2 cameraLockPos;

    private GameObject player;
    private cameraController cam;
    private bool playerTouching;

    public AudioSource sqweek;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.GetComponent<cameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f") && !locked && playerTouching && (Time.timeScale > 0))
        {
            if (cameraLockPos.x != 0)
            {
                cam.lockPosition = cameraLockPos;
                cam.locked = true;
            }
            else
                cam.locked = false;
            player.transform.position = new Vector3(exitDoor.transform.position.x, exitDoor.transform.position.y, player.transform.position.z);
            sqweek.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == player.GetComponent<Collider2D>())
            playerTouching = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider == player.GetComponent<Collider2D>())
            playerTouching = false;
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
