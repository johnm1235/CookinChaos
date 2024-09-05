using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState { Raw, Cut, Cooked }
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public Sprite cutIcon;
    public Sprite cookedIcon;
    public ItemState itemState;

}
