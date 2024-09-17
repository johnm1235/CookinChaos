using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixingStation : Station
{
    public List<Item> itemsInBowl = new List<Item>();
    public Transform bowlPosition;
    public GameObject ingredientsCanvasPrefab;
    private GameObject ingredientsCanvasInstance;

    public GameObject foodItemPrefab;

    public Transform tablePosition;
    public float itemHeightAboveTable = 1.5f;

    private const int maxIngredients = 6; // M�ximo de ingredientes permitidos

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem != null)
        {
            Item currentItem = PlayerInventory.Instance.currentItem;

            // Comprobar si el ingrediente es mezclable
            if (currentItem.itemState == ItemState.Cut || currentItem.itemState == ItemState.Cooked)
            {
                AddItemToBowl(currentItem);
                PlayerInventory.Instance.RemoveItem();
            }
            else
            {
                Debug.Log("El �tem no est� listo para ser mezclado.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && itemsInBowl.Count > 0)
        {
            // Si hay un plato y el bowl tiene ingredientes mezclados, se puede recoger
            CollectMixedItem();
        }
    }

    private void AddItemToBowl(Item item)
    {
        // Verificar si el n�mero de ingredientes en el bowl ha alcanzado el m�ximo permitido
        if (itemsInBowl.Count >= maxIngredients)
        {
            Debug.Log("No se pueden agregar m�s de 6 ingredientes.");
            return;
        }

        itemsInBowl.Add(item);
        PositionOnTable(item); // Usar PositionOnTable en lugar de PositionItemInBowl

        // Verificar si hay m�s de un �tem en el bol
        if (itemsInBowl.Count > 1)
        {
            CreateMixedItem();
        }
    }

    // Posicionar el �tem en la mesa
    private void PositionOnTable(Item ingredient)
    {
        Vector3 itemPosition = tablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        ingredient.transform.position = itemPosition;
        ingredient.gameObject.SetActive(true);
    }

    private Item CreateMixedItem()
    {
        // Verificar si ya existe un �tem mezclado en el bowl
        Item mixedItem = itemsInBowl.Find(i => i.itemName == "MixedItem");

        GameObject newCanvasInstance = null;
        Image[] ingredientIcons = null;

        if (mixedItem == null)
        {
            // Si no hay �tem mezclado, crear uno nuevo
            GameObject newItemObject = Instantiate(foodItemPrefab, bowlPosition.position, Quaternion.identity);
            mixedItem = newItemObject.GetComponent<Item>();
            mixedItem.itemName = "MixedItem";

            // Crear un nuevo canvas de ingredientes
            newCanvasInstance = Instantiate(ingredientsCanvasPrefab, mixedItem.transform.position, Quaternion.identity);
            newCanvasInstance.transform.SetParent(mixedItem.transform);

            // Ajustar la posici�n del canvas
            RectTransform canvasRectTransform = newCanvasInstance.GetComponent<RectTransform>();
            canvasRectTransform.anchoredPosition = new Vector2(0, 1); // Ajusta seg�n sea necesario

            newCanvasInstance.AddComponent<LookAtCamera>();

            // Obtener las im�genes dentro del nuevo Canvas
            ingredientIcons = newCanvasInstance.GetComponentsInChildren<Image>();
        }
        else
        {
            // Obtener el canvas existente en el �tem mezclado
            Canvas mixedItemCanvas = mixedItem.GetComponentInChildren<Canvas>();
            ingredientIcons = mixedItemCanvas.GetComponentsInChildren<Image>();

            // Verificar si el canvas ya tiene �conos ocupados
            newCanvasInstance = mixedItemCanvas.gameObject;

            if (newCanvasInstance.GetComponent<LookAtCamera>() == null)
            {
                newCanvasInstance.AddComponent<LookAtCamera>();
            }
        }

        // Desactivar visualmente solo los slots vac�os (sin desactivar el GameObject)
        foreach (Image icon in ingredientIcons)
        {
            if (icon.sprite == null)
            {
                icon.enabled = false; // Desactivar visualmente los slots vac�os, pero no el GameObject
            }
        }

        // Determinar el �ndice inicial basado en �conos ya ocupados
        int index = 0;
        foreach (Image icon in ingredientIcons)
        {
            if (icon.sprite != null)
            {
                icon.enabled = true;  // Asegurarse de que los �conos ya asignados est�n visibles
                index++; // Aumentar el �ndice si el slot ya est� ocupado
            }
        }

        // Guardar los �conos de los ingredientes mezclados
        bool hasTomato = false;
        bool hasLettuce = false;
        bool hasCheese = false;
        bool hasBread = false;

        foreach (Item item in itemsInBowl)
        {
            if (item.itemName == "MixedItem") continue;  // Ignorar el �tem mezclado en el bowl

            if (item.itemName == "Tomato") hasTomato = true;
            if (item.itemName == "Lettuce") hasLettuce = true;
            if (item.itemName == "Cheese") hasCheese = true;
            if (item.itemName == "Bread") hasBread = true;

            // Obtener el canvas del ingrediente
            Canvas ingredientCanvas = item.GetComponentInChildren<Canvas>();
            if (ingredientCanvas != null)
            {
                // Obtener la imagen del canvas del ingrediente
                Image ingredientImage = ingredientCanvas.GetComponentInChildren<Image>();
                if (ingredientImage != null && index < ingredientIcons.Length)
                {
                    // Activar el �cono cuando se asigna el sprite
                    ingredientIcons[index].sprite = ingredientImage.sprite;
                    ingredientIcons[index].enabled = true; // Asegurarse de que el slot est� visible
                    index++;
                }
            }

            // Destruir el ingrediente una vez a�adido
            Destroy(item.gameObject);
        }

        // Verificar combinaciones de ingredientes
        if (hasTomato && hasLettuce)
        {
            mixedItem.itemName = "Salad";
        }
        else if (hasTomato && hasCheese)
        {
            mixedItem.itemName = "TomatoCheese";
        }
        else if (hasBread && hasCheese)
        {
            mixedItem.itemName = "CheeseSandwich";
        }
        else if (hasTomato && hasLettuce && hasCheese)
        {
            mixedItem.itemName = "DeluxeSalad";
        }
        // Agrega m�s combinaciones seg�n sea necesario

        // Limpiar el bowl excepto el �tem mezclado
        itemsInBowl.Clear();
        itemsInBowl.Add(mixedItem);

        // Posicionar el �tem mezclado en la mesa
        PositionOnTable(mixedItem);

        return mixedItem;
    }

    private void CollectMixedItem()
    {
        if (itemsInBowl.Count == 1 && (itemsInBowl[0].itemName == "MixedItem" || itemsInBowl[0].itemName == "Salad" || itemsInBowl[0].itemName == "TomatoCheese" || itemsInBowl[0].itemName == "CheeseSandwich" || itemsInBowl[0].itemName == "DeluxeSalad"))
        {
            Item mixedItem = itemsInBowl[0];
            itemsInBowl.Clear();
            PlayerInventory.Instance.PickUpItem(mixedItem);
        }
    }
}
