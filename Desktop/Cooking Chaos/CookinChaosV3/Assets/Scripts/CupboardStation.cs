using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupboardStation : Station
{
    //Posición de la mesa 
    public Transform tablePosition;
    public float itemHeightAboveTable = 1.5f;

    //Item a cocinar
    private Item ingredient;

    //Variable para saber si hay un ingrediente en la mesa
    private bool IngredientDisponible = false;

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null)
        {
            if (IngredientDisponible == false)
            {
                ingredient = PlayerInventory.Instance.currentItem; 

                PositionOnTable();

                IngredientDisponible = true;
                PlayerInventory.Instance.RemoveItem(); 
            }
            else if (IngredientDisponible == true)
            {
                Debug.Log("There are an Ingredient");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerInventory.Instance.PickUpItem(ingredient);
            ingredient = null;
            IngredientDisponible = false;
        }
        else
        {
            Debug.Log("No item to cook or item is not cut.");
        }
    }

    private void PositionOnTable()
    {
        Vector3 itemPosition = tablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        ingredient.transform.position = itemPosition; 
        ingredient.gameObject.SetActive(true); 
    }
}
