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

    public void ResetItems()
    {
        for (int i = 0; i < items.Count; i++)
		{
			 items[i].available = true;
		}
    }

    public void RollSeen()
    {

    }


}
[System.Serializable]
public class Room
{
    public int availablePlaces = 3;
    public int availableCards = 3;

    public List<bool> place;
    public List<int> cardList;

    public void ResetRoom()
    {
        for (int i = 0; i < place.Count; i++)
        {
            place[i] = false;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i] = -1;
        }
        UpdateRoom();
    }

    public void UpdateRoom()
    {

    }

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