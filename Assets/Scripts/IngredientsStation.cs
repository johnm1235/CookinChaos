using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsStation : Station
{
    public Item foodItemPrefab;

    protected override void InteractWithStation()
    {
        // Verificar si el jugador ya tiene un ítem en su inventario
        if (PlayerInventory.Instance.currentItem == null)
        {
            Item newItem = Instantiate(foodItemPrefab, transform.position, Quaternion.identity);
            PlayerInventory.Instance.PickUpItem(newItem);
        }
        else
        {
            Debug.Log("El jugador ya tiene un ingrediente y no puede agarrar otro.");
        }
    }
}
