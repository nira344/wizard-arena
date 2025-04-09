using UnityEngine;
using System.Collections.Generic;

public class spawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float spawnCooldown;
    public int maxEnemies;

    public List<GameObject> spawnedEnemyObjects;
    public float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedEnemyObjects = new List<GameObject>();
        spawnedEnemyObjects.Capacity = maxEnemies;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Iterate through the list in reverse order to avoid skipping elements when removing them
        for (int i = spawnedEnemyObjects.Count - 1; i >= 0; i--)
        {
            // If the GameObject is missing (null), remove it from the list
            if (spawnedEnemyObjects[i] == null)
            {
                spawnedEnemyObjects.RemoveAt(i);
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (spawnedEnemyObjects.Count < maxEnemies)
            {
                GameObject goober = Instantiate(prefabs[0], gameObject.transform.position, Quaternion.identity);
                timer = spawnCooldown;
                spawnedEnemyObjects.Insert(0, goober);
            }
        }
    }
}
