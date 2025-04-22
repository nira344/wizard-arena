using UnityEngine;
using TMPro;


public class InventoryManager : MonoBehaviour
{
   public GameObject InventoryMenu;
   private bool menuActivated;
   public itemSlot[] itemSlot;
   public TextMeshProUGUI winText;
   public itemSlot equipSlot;
   public GameObject player;


   void Start()
   {
       winText.gameObject.SetActive(false);
       player = GameObject.FindGameObjectWithTag("Player");
   }


   void Update()
   {
       if (!winText.IsActive())
       {
           if (Input.GetKeyDown(KeyCode.Tab))
           {
               menuActivated = !menuActivated;
               InventoryMenu.SetActive(menuActivated);
               Time.timeScale = menuActivated ? 0 : 1;


                if (menuActivated)
                {
                    DeselectAllSlots();
                    if (itemSlot[0].isFull)
                    {
                        itemSlot[0].selectedShader.SetActive(true);
                        itemSlot[0].thisItemSelected = true;

                        if (itemSlot[0].ItemDescriptionNameText != null)
                            itemSlot[0].ItemDescriptionNameText.text = itemSlot[0].itemName;

                        if (itemSlot[0].ItemDescriptionText != null)
                            itemSlot[0].ItemDescriptionText.text = itemSlot[0].itemDescription;

                        if (itemSlot[0].itemDescriptionImage != null)
                            itemSlot[0].itemDescriptionImage.sprite = itemSlot[0].itemSprite != null ? itemSlot[0].itemSprite : itemSlot[0].emptySprite;
                    }
                }

           }
       }


       // Attach Lantern on Close
       if (!menuActivated && equipSlot.itemName == "Lantern")
       {
           var lantern = GameObject.Find("LanternObject");
           if (lantern != null)
           {
               lantern.SetActive(true);
               lantern.transform.SetParent(player.transform);
               lantern.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
           }
       }
   }


   public void AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, GameObject usableItemObject)
   {
       foreach (var slot in itemSlot)
       {
           if (slot.isFull && slot.itemName == itemName)
           {
               slot.quantity += quantity;
               slot.UpdateQuantityText();
               return;
           }
       }


       foreach (var slot in itemSlot)
       {
           if (!slot.isFull)
           {
               slot.AddItem(itemName, quantity, itemSprite, itemDescription);


               if (usableItemObject.TryGetComponent<IUsableItem>(out var usable))
               {
                   var usableType = usable.GetType();
                   if (!slot.gameObject.GetComponent(usableType))
                   {
                       slot.gameObject.AddComponent(usableType);
                   }
               }


               return;
           }
       }


       Debug.LogWarning("No empty inventory slot for: " + itemName);
   }


   public void DeselectAllSlots()
   {
       foreach (var slot in itemSlot)
       {
           slot.selectedShader.SetActive(false);
           slot.thisItemSelected = false;
       }


       equipSlot.selectedShader.SetActive(false);
       equipSlot.thisItemSelected = false;
   }


   public void EquipItem(itemSlot fromSlot)
   {
       if (equipSlot.isFull)
       {
           UnequipItem(); // Swap
       }


       equipSlot.AddItem(fromSlot.itemName, fromSlot.quantity, fromSlot.itemSprite, fromSlot.itemDescription);


       if (fromSlot.TryGetComponent<IUsableItem>(out var usable))
       {
           var usableType = usable.GetType();
           if (!equipSlot.gameObject.GetComponent(usableType))
           {
               equipSlot.gameObject.AddComponent(usableType);
           }
       }


       fromSlot.ClearSlot();
   }


   public void UnequipItem()
   {
       if (equipSlot.isFull)
       {
           AddItem(
               equipSlot.itemName,
               equipSlot.quantity,
               equipSlot.itemSprite,
               equipSlot.itemDescription,
               equipSlot.gameObject
           );


           equipSlot.ClearSlot();


           var lantern = GameObject.Find("LanternObject");
           if (lantern != null)
           {
               lantern.SetActive(false);
           }
       }
   }
}


