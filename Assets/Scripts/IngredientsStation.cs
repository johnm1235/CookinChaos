using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsStation : Station
{
    public Item foodItemPrefab;

    protected override void InteractWithStation()
    {
        if (PlayerInventory.Instance.currentItem == null)
        {
            Item newItem = Instantiate(foodItemPrefab, transform.position, Quaternion.identity);
            PlayerInventory.Instance.PickUpItem(newItem);
        }
    }
}
