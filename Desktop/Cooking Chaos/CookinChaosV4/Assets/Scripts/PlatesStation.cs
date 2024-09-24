using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesStation : Station
{
    public GameObject platePrefab;
    public int maxPlates = 2; // Número máximo de platos que se pueden generar
    public int currentPlates = 0; // Contador de platos generados
    public Transform plateSpawnPosition; // Posición base para generar los platos
    public float plateHeight = 0.1f; // Altura de cada plato

    public GameObject cleanPlate;

    protected override void InteractWithStation()
    {
        if (currentPlates < maxPlates)
        {
            if (!PlayerInventory.Instance.HasPlate())
            {
                Vector3 spawnPosition = plateSpawnPosition.position;
                spawnPosition.y += currentPlates * plateHeight; // Ajustar la posición vertical

                GameObject newPlate = Instantiate(platePrefab, spawnPosition, plateSpawnPosition.rotation);
                Plate plateComponent = newPlate.GetComponent<Plate>();
                plateComponent.UpdateModel(); // Asegurarse de que el modelo correcto esté activado
                PlayerInventory.Instance.PickUpItem(plateComponent);  // El jugador ahora tiene un plato.
                cleanPlate.SetActive(false);


                currentPlates++; // Incrementar el contador de platos generados
            }
            else
            {
                Debug.Log("Player already has a plate.");
            }
        }
        else
        {
            Debug.Log("Maximum number of plates reached.");
        }
    }

    public void GenerateCleanPlate()
    {
        if (cleanPlate != null)
            cleanPlate.SetActive(true);
    }

    public void PlateReturned()
    {
        if (currentPlates > 0)
        {
            currentPlates--; // Decrementar el contador de platos generados cuando un plato es devuelto
        }
    }
}
