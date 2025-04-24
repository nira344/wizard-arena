using UnityEngine;

public class ItemConsumer : MonoBehaviour
{
    private IUsableItem usableItem;

    void Start()
    {
        usableItem = GetComponent<IUsableItem>();
        if (usableItem == null)
        {
            Debug.LogWarning("ItemConsumer: No IUsableItem found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryConsume();
        }
    }

    public void TryConsume()
    {
        if (usableItem != null)
        {
            usableItem.Use(gameObject);
        }
        else
        {
            Debug.LogWarning("ItemConsumer: No IUsableItem found on this GameObject.");
        }
    }
}
