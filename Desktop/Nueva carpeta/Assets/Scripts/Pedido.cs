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
            Debug.LogError("Error: El prefab de comida no está asignado para este pedido.");
        }

        // Instancia la comida en la interfaz del pedido
        GameObject comidaVisual = Instantiate(comidaPrefab, transform);
        comidaVisual.transform.localPosition = Vector3.zero; // Ajusta la posición si es necesario
    }

    // Puedes añadir métodos adicionales aquí para la lógica de los pedidos
}
