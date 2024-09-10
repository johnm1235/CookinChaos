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
        itemsInBowl.Add(item);  
        PositionItemInBowl(item); 
    }

    // Posicionar el ítem en la estación de mezcla
    private void PositionItemInBowl(Item item)
    {
        item.transform.position = bowlPosition.position;
        item.transform.SetParent(bowlPosition);  // El bowl como padre del ingrediente
        item.gameObject.SetActive(true);
    }

    // Lógica para recoger los ítems mezclados
    private void CollectMixedItem()
    {
        Item mixedItem = CreateMixedItem();
        PlayerInventory.Instance.PickUpItem(mixedItem);  
        itemsInBowl.Clear(); 
        Destroy(ingredientsCanvasInstance);  
    }

    // Lógica para mezclar ingredientes
    private Item CreateMixedItem()
    {
        bool hasTomato = itemsInBowl.Exists(item => item.itemName == "Tomato");
        bool hasLettuce = itemsInBowl.Exists(item => item.itemName == "Lettuce");

        if (hasTomato && hasLettuce)
        {
            // Crear una ensalada si hay tomate y lechuga
            // Item mixedSalad = Instantiate(mixedSaladPrefab);
            // return mixedSalad;
        }

        return itemsInBowl[0];  // Devolver el primer ítem si no hay combinación específica
    }
}
