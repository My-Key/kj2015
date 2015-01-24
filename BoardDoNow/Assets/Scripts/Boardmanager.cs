using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boardmanager : MonoBehaviour {

    public static Boardmanager instance;
    public int greenDice = 0;
    public int redDice = 0;

    public List<Item> items;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void RollDices()
    {
        greenDice = Random.Range(1, 7);
        redDice = Random.Range(1, 7);
    }


    public void ReturnItem (int id)
    {
        items[id].available = true;
    }

    public int RollItem()
    {
        int item = -1;
        while(item == -1)
        {
            int newItem = Random.Range(0, items.Count+1);
            if (items[newItem].available)
            {
                items[newItem].available = false;
                item = newItem;
            }

        }
        return item;
    }

    public void ResetItems
    {
        for (int i = 0; i < items.Count; i++)
		{
			 
		}
    }
}

public class Room
{
    List<bool>
}


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
    Furniture = 5,
    Reversal = 6
}