using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour
{
	// Config
	public bool locked = false;
	public float followTightness;

	// Publicly accesible camera lock position
	public Vector2 lockPosition;

	// Player object
	public GameObject player;

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 desiredPos;

		// If camera is locked, instantly move the camera to that spot
		if (locked && (lockPosition != null))
		{
			transform.position = new Vector3(lockPosition.x, lockPosition.y, transform.position.z);
		}
		// If the camera is not locked, lerp to the player's approximate position
		else
		{
			float playerVelocityY = player.GetComponent<Rigidbody2D>().linearVelocityY;
			desiredPos = new Vector3(player.transform.position.x, player.transform.position.y + 2 + (playerVelocityY / 8), gameObject.transform.position.z);
			transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followTightness);
		}
	}
}
