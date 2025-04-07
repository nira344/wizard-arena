using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject fireballPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // Corrected the typo here
            var fireball = Instantiate(fireballPrefab, transform.position, transform.rotation);  // Instantiate the fireball
        }
    }
}
