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

            Debug.Log("Ingrediente eliminado en la estación de basura.");
        }
        else
        {
            Debug.Log("No hay ningún ítem en el inventario del jugador.");
        }
    }
}
