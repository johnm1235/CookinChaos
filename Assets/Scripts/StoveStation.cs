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

    // Posición de la mesadonde se mostrará el alimento.
    public Transform stoveTablePosition;
    public float itemHeightAboveTable = 1.5f;

    protected override void InteractWithStation()
    {
        // Verificar si el jugador tiene un ítem cortado para cocinar
        if (PlayerInventory.Instance.currentItem != null && PlayerInventory.Instance.currentItem.itemState == ItemState.Cut)
        {
            if (!isCooking && IngredientDisponible == false)
            {
                cookingItem = PlayerInventory.Instance.currentItem;
                PositionOnTable();
                IngredientDisponible = true;
                StartCoroutine(CookFood());
                PlayerInventory.Instance.RemoveItem();
            }
        }
        // Verificar si la comida ya está cocinada y el jugador tiene un plato
        else if (cookingItem != null && cookingItem.itemState == ItemState.Cooked && PlayerInventory.Instance.HasPlate() && Input.GetKeyDown(KeyCode.Space))
        {
            PlayerInventory.Instance.PickUpItem(cookingItem);
       //     PlayerInventory.Instance.RemovePlate(); // El jugador ya no tiene el plato después de agarrar la comida.
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
    }

    private void CompleteCooking()
    {
        isCooking = false;

        // Cambiar el estado del ítem a cocinndo
        cookingItem.ChangeState(ItemState.Cooked);

        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        cookingItem.transform.position = itemPosition;

        cookingItem.gameObject.SetActive(true);
        cookingProgressBar.gameObject.SetActive(false);
       // PlayerInventory.Instance.inventoryImage.sprite = cookingItem.cookedIcon;
        Debug.Log("Cooking complete and placed on Stove Station.");
    }


    private void PositionOnTable()
    {
        Vector3 itemPosition = stoveTablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        cookingItem.transform.position = itemPosition;
        cookingItem.transform.SetParent(stoveTablePosition);  // Asegura que el ítem quede en la mesa.
    }

}
