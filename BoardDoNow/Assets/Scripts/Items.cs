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
    Paintings = 0,
    Antiques = 1,
    Jewelry = 2,
    Book = 3,
    Furniture = 4,
    QuestionMark = 5
}
