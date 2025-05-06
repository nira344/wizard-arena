using UnityEngine;
using TMPro;

public class ShopItemObject : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public GameObject usableItemPrefab;
    public int soulCost = 10;

    public TextMeshPro costText;

    private InventoryManager inventoryManager;
    private bool playerInRange = false;

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();

        // Set the cost text (from child TextMeshPro)
        if (costText == null)
        {
            Transform costTextTransform = transform.Find("ShopCostText");
            if (costTextTransform != null)
                costText = costTextTransform.GetComponent<TextMeshPro>();
        }

        if (costText != null)
            costText.text = soulCost + " Souls";

        // Set the sprite on the ShopItemImage child
        Transform imageTransform = transform.Find("ShopItemImage");
        if (imageTransform != null)
        {
            SpriteRenderer imageRenderer = imageTransform.GetComponent<SpriteRenderer>();
            if (imageRenderer != null && itemSprite != null)
            {
                imageRenderer.sprite = itemSprite;
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed near shop item.");

            if (SoulManager.Instance.soulEssence >= soulCost)
            {
                SoulManager.Instance.SpendSouls(soulCost);
                inventoryManager.AddItem(itemName, 1, itemSprite, itemDescription, usableItemPrefab);
                Debug.Log("Purchased: " + itemName);
            }
            else
            {
                Debug.Log("Not enough souls for: " + itemName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered shop range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left shop range.");
        }
    }
}
