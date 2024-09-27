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

    private const int maxIngredients = 3; // Máximo de ingredientes permitidos

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
                Debug.Log("El ítem no está listo para ser mezclado.");
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
        // Verificar si el número de ingredientes en el bowl ha alcanzado el máximo permitido
        if (itemsInBowl.Count >= maxIngredients)
        {
            Debug.Log("No se pueden agregar más de 6 ingredientes.");
            return;
        }

        itemsInBowl.Add(item);
        PositionOnTable(item); // Usar PositionOnTable en lugar de PositionItemInBowl

        // Verificar si hay más de un ítem en el bol
        if (itemsInBowl.Count > 1)
        {
            CreateMixedItem();
        }
    }

    // Posicionar el ítem en la mesa
    private void PositionOnTable(Item ingredient)
    {
        Vector3 itemPosition = tablePosition.position;
        itemPosition.y += itemHeightAboveTable;
        ingredient.transform.position = itemPosition;
        ingredient.gameObject.SetActive(true);
    }

    private Item CreateMixedItem()
    {
        // Verificar si ya existen ítems mezclados en el bowl
        List<Item> mixedItems = itemsInBowl.FindAll(i => i.itemName == "MixedItem" || i.itemName == "Salad" || i.itemName == "TomatoCheese" || i.itemName == "CheeseSalad" || i.itemName == "DeluxeSalad");

        GameObject newCanvasInstance = null;
        Image[] ingredientIcons = null;
        Item mixedItem = null;

        if (mixedItems.Count == 0)
        {
            // Si no hay ítem mezclado, crear uno nuevo
            GameObject newItemObject = Instantiate(foodItemPrefab, bowlPosition.position, Quaternion.identity);
            mixedItem = newItemObject.GetComponent<Item>();
            mixedItem.itemName = "MixedItem";

            // Crear un nuevo canvas de ingredientes
            newCanvasInstance = Instantiate(ingredientsCanvasPrefab, mixedItem.transform.position, Quaternion.identity);
            newCanvasInstance.transform.SetParent(mixedItem.transform);

            // Ajustar la posición del canvas
            RectTransform canvasRectTransform = newCanvasInstance.GetComponent<RectTransform>();
            canvasRectTransform.anchoredPosition = new Vector2(0, 1); // Ajusta según sea necesario

            newCanvasInstance.AddComponent<LookAtCamera>();

            // Obtener las imágenes dentro del nuevo Canvas
            ingredientIcons = newCanvasInstance.GetComponentsInChildren<Image>();
        }
        else
        {
            // Obtener el primer ítem mezclado existente
            mixedItem = mixedItems[0];

            // Obtener el canvas existente en el ítem mezclado
            Canvas mixedItemCanvas = mixedItem.GetComponentInChildren<Canvas>();
            ingredientIcons = mixedItemCanvas.GetComponentsInChildren<Image>();

            // Verificar si el canvas ya tiene íconos ocupados
            newCanvasInstance = mixedItemCanvas.gameObject;

            if (newCanvasInstance.GetComponent<LookAtCamera>() == null)
            {
                newCanvasInstance.AddComponent<LookAtCamera>();
            }
        }

        // Desactivar visualmente solo los slots vacíos (sin desactivar el GameObject)
        foreach (Image icon in ingredientIcons)
        {
            if (icon.sprite == null)
            {
                icon.enabled = false; // Desactivar visualmente los slots vacíos, pero no el GameObject
            }
        }

        // Determinar el índice inicial basado en íconos ya ocupados
        int index = 0;
        foreach (Image icon in ingredientIcons)
        {
            if (icon.sprite != null)
            {
                icon.enabled = true;  // Asegurarse de que los íconos ya asignados estén visibles
                index++; // Aumentar el índice si el slot ya está ocupado
            }
        }

       

        // Lista para almacenar los ingredientes mezclados
        List<string> mixedIngredients = new List<string>();

        foreach (Item item in itemsInBowl)
        {
            mixedIngredients.Add(item.itemName);
            if (item.itemName == "MixedItem" || item.itemName == "Salad" || item.itemName == "TomatoCheese" || item.itemName == "CheeseSalad" || item.itemName == "DeluxeSalad") continue;  // Ignorar el ítem mezclado en el bowl

            // Obtener el canvas del ingrediente
            Canvas ingredientCanvas = item.GetComponentInChildren<Canvas>();
            if (ingredientCanvas != null)
            {
                // Obtener la imagen del canvas del ingrediente
                Image ingredientImage = ingredientCanvas.GetComponentInChildren<Image>();
                if (ingredientImage != null && index < ingredientIcons.Length)
                {
                    // Activar el ícono cuando se asigna el sprite
                    ingredientIcons[index].sprite = ingredientImage.sprite;
                    ingredientIcons[index].enabled = true; // Asegurarse de que el slot esté visible
                    index++;
                }
            }

            // Destruir el ingrediente una vez añadido
            Destroy(item.gameObject);
        }

        // Verificar combinaciones de ingredientes
        if (mixedIngredients.Contains("Tomato") && mixedIngredients.Contains("Lettuce") && mixedIngredients.Contains("Cheese"))
        {
            mixedItem.itemName = "DeluxeSalad";
        }
        else if (mixedIngredients.Contains("Tomato") && mixedIngredients.Contains("Lettuce"))
        {
            mixedItem.itemName = "Salad";
        }
        else if (mixedIngredients.Contains("Tomato") && mixedIngredients.Contains("Cheese"))
        {
            mixedItem.itemName = "TomatoCheese";
        }
        else if (mixedIngredients.Contains("Lettuce") && mixedIngredients.Contains("Cheese"))
        {
            mixedItem.itemName = "CheeseSalad";
        }
        else if (mixedIngredients.Contains("Salad") && mixedIngredients.Contains("Cheese"))
        {
            mixedItem.itemName = "DeluxeSalad";
        }
        else if (mixedIngredients.Contains("TomatoCheese") && mixedIngredients.Contains("Lettuce"))
        {
            mixedItem.itemName = "DeluxeSalad";
        }
        else if (mixedIngredients.Contains("CheeseSalad") && mixedIngredients.Contains("Tomato"))
        {
            mixedItem.itemName = "DeluxeSalad";
        }
        // Agrega más combinaciones según sea necesario

        // Limpiar el bowl excepto el ítem mezclado
        itemsInBowl.Clear();
        itemsInBowl.Add(mixedItem);

        // Posicionar el ítem mezclado en la mesa
        PositionOnTable(mixedItem);

        return mixedItem;
    }

    private void CollectMixedItem()
    {
        if (PlayerInventory.Instance.HasPlate() )
        {
            if (itemsInBowl.Count == 1 && (itemsInBowl[0].itemName == "MixedItem" || itemsInBowl[0].itemName == "Salad" || itemsInBowl[0].itemName == "TomatoCheese" || itemsInBowl[0].itemName == "CheeseSalad" || itemsInBowl[0].itemName == "DeluxeSalad"))
            {
                Item mixedItem = itemsInBowl[0];
                itemsInBowl.Clear();
                PlayerInventory.Instance.PickUpItem(mixedItem);
            }

        }
        else
        {
            Debug.Log("No hay plato para recoger el ítem mezclado.");
        }
    }
}
