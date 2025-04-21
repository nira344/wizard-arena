using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour
{
	// Config
	public bool follow;
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
		// Calulate and move to midpoint of ships
		if (follow) // Check config
		{
			float playerVelocityY = player.GetComponent<Rigidbody2D>().linearVelocityY;
			Vector3 desiredPos = new Vector3(player.transform.position.x, player.transform.position.y + 2 + (playerVelocityY / 8), gameObject.transform.position.z);
			transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * followTightness);
		}
	}
}
