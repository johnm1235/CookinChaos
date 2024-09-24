using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceStation : Station
{
    public Transform stoveTablePosition;
    public float itemHeightAboveTable = 1.5f;

    // Referencia a DirtyPlatesStation
    public DirtyPlatesStation dirtyPlatesStation;

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Cooked)
        {
            // Obtener el Item actual
            Item deliveredItem = PlayerInventory.Instance.currentItem;
            string deliveredItemName = deliveredItem.itemName;

            // Verificar si el item entregado coincide con alg�n pedido
            bool pedidoEntregado = Orders.Instance.VerificarYEliminarPedido(deliveredItemName);

            if (pedidoEntregado)
            {
                Debug.Log("Pedido entregado correctamente: " + deliveredItemName);
                PositionOnTable(deliveredItem);
                PlayerInventory.Instance.RemoveItem();
                PlayerInventory.Instance.RemovePlate();
                StartCoroutine(enumerator());
                Destroy(deliveredItem.gameObject);

                // Llamar al m�todo GenerateDirtyPlate de DirtyPlatesStation
                if (dirtyPlatesStation != null)
                {
                    dirtyPlatesStation.GenerateDirtyPlate();
                }
                else
                {
                    Debug.LogWarning("DirtyPlatesStation reference is not set.");
                }
            }
            else
            {
                Debug.Log("El item no coincide con ning�n pedido.");
            }
        }
        else
        {
            Debug.Log("No hay item para entregar o el item no est� cocinado.");
        }
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(3);
    }

    private void PositionOnTable(Item ingredient)
    {
        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        ingredient.transform.position = itemPosition;
    }
}
