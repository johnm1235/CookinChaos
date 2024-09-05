using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceStation : Station
{
    public Item ingredient;
    public bool IngredientDisponible = false;

    public Transform stoveTablePosition;
    public float itemHeightAboveTable = 1.5f;

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Cooked)
        {
            if (IngredientDisponible == false)
            {
                ingredient = PlayerInventory.Instance.currentItem; 

                PositionOnTable();

                IngredientDisponible = true;
                PlayerInventory.Instance.RemoveItem(); 

                StartCoroutine(enumerator());
            }

            Destroy(ingredient.gameObject);
        }
        else
        {
            Debug.Log("No item to deliver or item is not cooked.");
        }
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(3);
    }

    private void PositionOnTable()
    {

        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        ingredient.transform.position = itemPosition; 


    }
}
