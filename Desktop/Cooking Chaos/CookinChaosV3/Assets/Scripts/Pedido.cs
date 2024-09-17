using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedido : MonoBehaviour
{
    public string comidaRequerida; // Nombre del item requerido para este pedido
    public GameObject comidaPrefab; // Prefab que representa la comida en el pedido
    public bool completado = false;
    public bool expirado = false;

    void Start()
    {
        if (comidaPrefab == null)
        {
            Debug.LogError("Error: El prefab de comida no est� asignado para este pedido.");
        }

        // Instancia la comida en la interfaz del pedido
        GameObject comidaVisual = Instantiate(comidaPrefab, transform);
        comidaVisual.transform.localPosition = Vector3.zero; // Ajusta la posici�n si es necesario
    }

    // Puedes a�adir m�todos adicionales aqu� para la l�gica de los pedidos
}
