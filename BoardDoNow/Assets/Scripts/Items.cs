using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public class Item
{
    public ItemType itemType;
    public int price = 0;
    public bool available = true;
}

public enum ItemType
{
    Paintings = 1,
    Antiques = 2,
    Jewelry = 3,
    Book = 4,
    Furniture = 5
    //Reversal = 6
}
