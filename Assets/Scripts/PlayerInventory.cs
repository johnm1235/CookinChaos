using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public Item currentItem;
    public Image inventoryImage;

    // Transform de la mano del personaje donde se colocará el ítem
    public Transform handPosition;

    // Variable para saber si el jugador tiene algo en la mano
    private bool isHoldingItem = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickUpItem(Item item)
    {
        currentItem = item;
        isHoldingItem = true;
        PositionInHand(); 
        UpdateInventoryImage();

    }

    public void RemoveItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            currentItem = null;
            inventoryImage.sprite = null; 
            isHoldingItem = false;
        }
    }

    private void PositionInHand()
    {
        currentItem.gameObject.SetActive(true); 
        if (currentItem != null)
        {
            currentItem.transform.SetParent(handPosition);

            currentItem.transform.localPosition = Vector3.zero; 
            currentItem.transform.localRotation = Quaternion.identity;


        }
    }

    public void UpdateInventoryImage()
    {
        if (currentItem != null)
        {
            switch (currentItem.itemState)
            {
                case ItemState.Raw:
                    inventoryImage.sprite = currentItem.itemIcon;
                    break;
                case ItemState.Cut:
                    inventoryImage.sprite = currentItem.cutIcon;
                    break;
                case ItemState.Cooked:
                    inventoryImage.sprite = currentItem.cookedIcon;
                    break;
            }
        }
        else
        {
            inventoryImage.sprite = null;
        }
    }
}
