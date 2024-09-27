using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlateState { Clean, Dirty }

public class Plate : MonoBehaviour
{
    // Modelos 3D para los diferentes estados
    public GameObject cleanModel;
    public GameObject dirtyModel;

    public PlateState itemState = PlateState.Clean; // Estado inicial configurado a Clean

    // Representa si el jugador tiene un plato
    public bool hasPlate = false;

    public bool isClean;

    // Icono del plato (si se quiere mostrar en el inventario)
    public Sprite plateIcon;

    public void Start()
    {
        UpdateModel(); // Asegurarse de que el modelo correcto esté activado al inicio
    }

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

    public void ChangeState(PlateState newState)
    {
        itemState = newState; // Cambiar el estado
        UpdateModel();
    }

    public void UpdateModel()
    {
        // Actualizar los modelos según el estado del plato
        if (itemState == PlateState.Clean)
        {
            cleanModel.SetActive(true);
            dirtyModel.SetActive(false);
            Debug.Log("Plate is clean. Activating clean model.");
            isClean = true;
        }
        else if (itemState == PlateState.Dirty)
        {
            cleanModel.SetActive(false);
            dirtyModel.SetActive(true);
            Debug.Log("Plate is dirty. Activating dirty model.");
            isClean = false;
        }
    }
}
