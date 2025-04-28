using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour
{
	// Config
	public bool locked = false;
	public Vector2 lockPosition;
	public float followTightness;

	// Ship Targets
	public GameObject player;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		Vector3 desiredPos;

		// Calulate and move to midpoint of ships
		if (locked && (lockPosition != null)) // Check config
		{
			desiredPos = new Vector3(lockPosition.x, lockPosition.y, transform.position.z);
		}
		else
		{
			float playerVelocityY = player.GetComponent<Rigidbody2D>().linearVelocityY;
			desiredPos = new Vector3(player.transform.position.x, player.transform.position.y + 2 + (playerVelocityY / 8), gameObject.transform.position.z);
		}

		transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followTightness);
	}
}
