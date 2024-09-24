using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveStation : Station
{
    //Item to cook
    private Item cookingItem;

    //Cooking
    private bool isCooking = false;
    public float cookingTime = 5f;
    private float currentCookingTime = 0f;

    public bool IngredientDisponible = false;

    //ProgressBar
    public Slider cookingProgressBar;

    // Posici�n de la mesa donde se mostrar� el alimento.
    public Transform stoveTablePosition;
    public float itemHeightAboveTable = 1.5f;

    public FloatingStation floatingStation;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cookingSound;



    protected override void InteractWithStation()
    {
        
        // Verificar si el jugador tiene un �tem cortado para cocinar
        if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Cut)
        {
            
            if (!isCooking && IngredientDisponible == false)
            {
                cookingItem = PlayerInventory.Instance.currentItem;
                cookingItem.transform.SetParent(stoveTablePosition);

                PositionOnTable();
                IngredientDisponible = true;
                StartCoroutine(CookFood());
                PlayerInventory.Instance.RemoveItem();
            }
        }
        // Verificar si la comida ya est� cocinada y el jugador tiene un plato
        else if (cookingItem != null && cookingItem.itemState == ItemState.Cooked && PlayerInventory.Instance.HasPlate() && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerInventory.Instance.PickUpItem(cookingItem);
            cookingItem = null;
            IngredientDisponible = false;
        }
        else
        {
            Debug.Log("No item to cook or item is not cut.");
        }
    }

    private IEnumerator CookFood()
    {
        audioSource.PlayOneShot(cookingSound);
        if (floatingStation)
        floatingStation.isCooking = true;
        isCooking = true;
        currentCookingTime = 0f;
        cookingProgressBar.gameObject.SetActive(true);
        cookingProgressBar.value = 0f;

        while (currentCookingTime < cookingTime)
        {
            currentCookingTime += Time.deltaTime;
            cookingProgressBar.value = currentCookingTime / cookingTime;
            yield return null;
        }

        CompleteCooking();
        audioSource.Stop();
    }

    private void CompleteCooking()
    {
        if (floatingStation)
            floatingStation.isCooking = false;
        isCooking = false;

        // Cambiar el estado del �tem a cocinado
        cookingItem.ChangeState(ItemState.Cooked);

        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        cookingItem.transform.position = itemPosition;

        // Establecer la comida como hija de la estaci�n
        cookingItem.transform.SetParent(stoveTablePosition);

        cookingItem.gameObject.SetActive(true);
        cookingProgressBar.gameObject.SetActive(false);

        Debug.Log("Cooking complete and placed on Stove Station.");
    }

    private void PositionOnTable()
    {
        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        cookingItem.transform.position = itemPosition;

        // Establecer la comida como hija de la estaci�n cuando se coloca en la mesa
      //  cookingItem.transform.SetParent(stoveTablePosition);
    }
}
