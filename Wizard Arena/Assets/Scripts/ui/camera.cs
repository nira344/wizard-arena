using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour
{
	// Configuration
	public bool teleport = false;
	public float followTightness;

	// Publicly accesible camera locking variables
	public Vector2 lockPosition;
	public bool locked = false;

	// Player object
	private GameObject player;

	void Start()
	{
		// Find player
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 desiredPos;
		float playerVelocityY = player.GetComponent<Rigidbody2D>().linearVelocityY;

		// If the camera is moving out of a room, teleport for 1 frame only
		if (teleport)
		{
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2 + (playerVelocityY / 8), transform.position.z);
			teleport = false;
		}
		// If camera is locked, instantly move the camera to that spot
		else if (locked && lockPosition != null)
		{
			transform.position = new Vector3(lockPosition.x, lockPosition.y, transform.position.z);
		}
		// If the camera is not locked, lerp to the player's approximate position
		else
		{
			desiredPos = new Vector3(player.transform.position.x, player.transform.position.y + 2 + (playerVelocityY / 8), gameObject.transform.position.z);
			transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followTightness);
		}
	}
}
