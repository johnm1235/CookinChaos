using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // Representa si el jugador tiene un plato
    public bool hasPlate = false;

    // Icono del plato (si se quiere mostrar en el inventario)
    public Sprite plateIcon;

    public void PickUpPlate()
    {
        hasPlate = true;
        Debug.Log("Plate picked up.");
    }

    public void RemovePlate()
    {
        hasPlate = false;
        Debug.Log("Plate removed.");

        Destroy(gameObject);
    }
}
