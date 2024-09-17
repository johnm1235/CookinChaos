using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashStation : Station
{
    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null)
        {
            // Obtener el ingrediente actual del inventario del jugador
            Item ingredient = PlayerInventory.Instance.currentItem;

            // Destruir el ingrediente
            Destroy(ingredient.gameObject);

            // Remover el ingrediente del inventario del jugador
            PlayerInventory.Instance.RemoveItem();

            Debug.Log("Ingrediente eliminado en la estaci�n de basura.");
        }
        else
        {
            Debug.Log("No hay ning�n �tem en el inventario del jugador.");
        }
    }
}
