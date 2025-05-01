using UnityEngine;
using System.Collections.Generic;

public class deathsoundmanager : MonoBehaviour
{
    
    public AudioClip destroySound;

    private static deathsoundmanager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Register(GameObject obj)
    {
        if (instance == null) return;

        if (obj.CompareTag("Enemy"))
        {
            // Dynamically attach the watcher to detect destruction
            if (obj.GetComponent<deathwatcher>() == null)
            {
                var watcher = obj.AddComponent<deathwatcher>();
                watcher.Setup(instance.destroySound);
            }
        }
    }
    
    void update(GameObject obj)
    {
        if (obj.ComparTag("Enemy"))
        {
            if (obj.GetComponent<deathwatcher>() == null)
            {
                var watcher = obj.AddComponent<deathwatcher>();
                watcher.Setup(instance.destroySound); 
            }
        }
    }
}
