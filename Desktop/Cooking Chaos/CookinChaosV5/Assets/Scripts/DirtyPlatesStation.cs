using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlatesStation : Station
{
    public GameObject dirtyPlatePrefab;
    public Transform plateSpawnPosition;
    public float heightAboveStation = 1.5f;

    private GameObject spawnedPlate;

    protected override void InteractWithStation()
    {
        if (spawnedPlate != null && PlayerInventory.Instance != null && !PlayerInventory.Instance.HasPlate())
        {
            Plate plateComponent = spawnedPlate.GetComponent<Plate>();
            if (plateComponent != null)
            {
                PlayerInventory.Instance.PickUpItem(plateComponent);
                spawnedPlate = null; // Limpiar la referencia después de recoger el plato
                Debug.Log("Dirty plate picked up.");
            }
        }
    }

    public void GenerateDirtyPlate()
    {
        if (dirtyPlatePrefab != null && plateSpawnPosition != null)
        {
            Vector3 spawnPosition = plateSpawnPosition.position;
            spawnPosition.y += heightAboveStation;

            spawnedPlate = Instantiate(dirtyPlatePrefab, spawnPosition, plateSpawnPosition.rotation);
            Plate plateComponent = spawnedPlate.GetComponent<Plate>();
            plateComponent.ChangeState(PlateState.Dirty);
            plateComponent.UpdateModel(); // Asegurarse de que el modelo correcto esté activado
            Debug.Log("Dirty plate spawned.");
        }
        else
        {
            Debug.LogWarning("Dirty plate prefab or spawn position is not set.");
        }
    }
}
