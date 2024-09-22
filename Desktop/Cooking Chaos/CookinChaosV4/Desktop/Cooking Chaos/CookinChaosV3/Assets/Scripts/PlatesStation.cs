using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesStation : Station
{
    public GameObject platePrefab;

    protected override void InteractWithStation()
    {
        if (!PlayerInventory.Instance.HasPlate())
        {
            GameObject newPlate = Instantiate(platePrefab, PlayerInventory.Instance.handPosition);
            Plate plateComponent = newPlate.GetComponent<Plate>();
            PlayerInventory.Instance.PickUpItem(plateComponent);  // El jugador ahora tiene un plato.
        }
        else
        {
            Debug.Log("Player already has a plate.");
        }
        if (PlayerInventory.Instance.HasPlate())
        {
           // PlayerInventory.Instance.RemovePlate();  // El jugador suelta el plato.
        }

    }
}
