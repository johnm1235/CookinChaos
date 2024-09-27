using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WashingStation : Station
{
    public bool isWashing = false;
    public float washingTime = 3f;
    private float currentWashingTime = 0f;

    // Añadir slider
    public Slider sliderWash;

    // Distancia máxima para interactuar con la estación
    public float interactionDistance = 2f;

    // Referencia a PlatesStation
    public PlatesStation platesStation;

    protected override void InteractWithStation()
    {
        // Verificar si el jugador está cerca de la estación
        if (IsPlayerClose())
        {
            if (PlayerInventory.Instance.HasDirtyPlate() && !isWashing)
            {
                StartCoroutine(WashPlate());
            }
            else if (!PlayerInventory.Instance.HasPlate())
            {
                Debug.Log("Player doesn't have a plate.");
            }
        }
    }

    private IEnumerator WashPlate()
    {
        isWashing = true;
        currentWashingTime = 0f;
        sliderWash.gameObject.SetActive(true);
        sliderWash.value = currentWashingTime / washingTime;

        while (currentWashingTime < washingTime)
        {
            if (Input.GetKey(KeyCode.Space) && IsPlayerClose())
            {
                currentWashingTime += Time.deltaTime;
                sliderWash.value = currentWashingTime / washingTime;
            }
            else if (!IsPlayerClose())
            {
                Debug.Log("Player moved away from the washing station.");
                isWashing = false;
                sliderWash.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }

        isWashing = false;
        PlayerInventory.Instance.RemovePlate();  // El jugador suelta el plato.
        Debug.Log("Plate washed.");
        sliderWash.gameObject.SetActive(false);

        // Llamar al método PlateReturned de PlatesStation
        if (platesStation != null)
        {
            platesStation.PlateReturned();
            platesStation.GenerateCleanPlate();
        }
        else
        {
            Debug.LogWarning("PlatesStation reference is not set.");
        }
    }

    private bool IsPlayerClose()
    {
        // Verificar la distancia entre el jugador y la estación de lavado
        return Vector3.Distance(PlayerInventory.Instance.transform.position, transform.position) <= interactionDistance;
    }
}

