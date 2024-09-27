using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public Item currentItem;
    public Plate currentPlate; // Referencia al plato que tiene el jugador, si tiene uno.
                                   //  public Image inventoryImage;
    public Transform handPosition;

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
        PositionInHand();
    }

    public void PickUpItem(Plate plate)
    {
        currentPlate = plate;
        currentPlate.PickUpPlate();
        PositionInHand();
    }

    public void RemoveItem()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(null);
            currentItem = null;
            //    inventoryImage.sprite = null;
        }
    }

    public void RemovePlate()
    {
        if (currentPlate != null)
        {
            currentPlate.RemovePlate();
            currentPlate = null;
        }
    }
    public bool HasPlate()
    {
        return currentPlate != null && currentPlate.hasPlate && currentPlate.isClean;
    }


    public bool HasDirtyPlate()
    {
        return currentPlate != null && currentPlate.hasPlate && currentPlate.isClean == false;
    }

    private void PositionInHand()
    {
        if (currentItem != null)
        {
            currentItem.transform.SetParent(handPosition);
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;
            currentItem.gameObject.SetActive(true);
        }

        if (currentPlate != null)
        {
            currentPlate.transform.SetParent(handPosition);
            currentPlate.transform.localPosition = Vector3.zero;
            currentPlate.transform.localRotation = Quaternion.identity;
            currentPlate.gameObject.SetActive(true);
        }

    }
}
