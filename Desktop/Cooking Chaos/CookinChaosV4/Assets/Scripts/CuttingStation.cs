using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingStation : Station
{
    private Item cuttingItem;

    //Cutting
    public bool isCutting = false;
    public float cuttingTime = 3f;
    private float currentCuttingTime = 0f;

    public bool IngredientDisponible = false;

    public Transform tablePosition;
    public float itemHeightAboveTable = 1.5f;

    //Anadir slider
    public Slider sliderCut;

    // Distancia máxima para interactuar con la estación
    public float interactionDistance = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cuttingSound;

    protected override void InteractWithStation()
    {
        // Verificar si el jugador está cerca de la estación
        if (IsPlayerClose())
        {
            // Verificar si el jugador tiene un ingrediente crudo y la estación está libre
            if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Raw && cuttingItem == null)
            {
                cuttingItem = PlayerInventory.Instance.currentItem;

                ShowIngredient();
                IngredientDisponible = true;
                StartCoroutine(CutFood());
            }
            // Verificar si el ingrediente en la estación está cortado y el jugador no tiene un ítem
            else if (cuttingItem != null && cuttingItem.itemState == ItemState.Cut && PlayerInventory.Instance.currentItem == null)
            {
                PlayerInventory.Instance.PickUpItem(cuttingItem);
                cuttingItem = null;
                IngredientDisponible = false;
            }
            else
            {
                Debug.Log("No item to cut, item is not raw, or player already has an item.");
            }
        }
    }

    private void ShowIngredient()
    {
        // Mostrar el ingrediente en la estación de corte
        cuttingItem.gameObject.transform.position = transform.position;
        cuttingItem.gameObject.SetActive(true);
        PositionOnTable();
        PlayerInventory.Instance.RemoveItem();
    }

    private IEnumerator CutFood()
    {
        audioSource.PlayOneShot(cuttingSound);
        isCutting = true;
        currentCuttingTime = 0f; // Reiniciar el tiempo de corte
        sliderCut.gameObject.SetActive(true);
        sliderCut.value = currentCuttingTime / cuttingTime;

        while (currentCuttingTime < cuttingTime)
        {
            if (Input.GetKey(KeyCode.Space) && IsPlayerClose())
            {
                currentCuttingTime += Time.deltaTime;
                sliderCut.value = currentCuttingTime / cuttingTime;
            }
            yield return null;
        }

        isCutting = false;
        // Cambiar el estado del ítem a cortado
        cuttingItem.ChangeState(ItemState.Cut);
        // Actualizar el ícono y el modelo del ingrediente
        cuttingItem.CreateWorldIconBasedOnState();
        sliderCut.gameObject.SetActive(false);

        Debug.Log("Item cut and placed on Cutting Station.");

        audioSource.Stop();
    }

    private void PositionOnTable()
    {
        Vector3 itemPosition = tablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        cuttingItem.transform.position = itemPosition;
        cuttingItem.gameObject.SetActive(true);
    }

    private bool IsPlayerClose()
    {
        // Verificar la distancia entre el jugador y la estación de corte
        return Vector3.Distance(PlayerInventory.Instance.transform.position, transform.position) <= interactionDistance;
    }
}
