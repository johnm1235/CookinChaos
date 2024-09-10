using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingStation : Station
{
    private Item cuttingItem;

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Raw)
        {
            cuttingItem = PlayerInventory.Instance.currentItem; 
            StartCoroutine(CutFood());
        }
        else if (cuttingItem != null && cuttingItem.itemState == ItemState.Cut)
        {
        
            PlayerInventory.Instance.PickUpItem(cuttingItem);
            cuttingItem.gameObject.SetActive(false);
            cuttingItem = null;
        }
        else
        {
            Debug.Log("No item to cut or item is not raw.");
        }
    }

    private IEnumerator CutFood()
    {
        yield return new WaitForSeconds(3);

        // Cambiar el estado del ítem a cortado
        cuttingItem.ChangeState(ItemState.Cut);

        cuttingItem.gameObject.transform.position = transform.position;
        cuttingItem.gameObject.SetActive(true);
        PlayerInventory.Instance.RemoveItem();
        Debug.Log("Item cut and placed on Cutting Station.");
    }

}
