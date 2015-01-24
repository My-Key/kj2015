using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[System.Serializable]
public class Room
{
    public bool isHall = false;
    public int availablePlaces = 3;
    public int availableCards = 3;

    public int placeTaken;
    public List<int> cardList;

    public List<GameObject> placGO;
    public List<GameObject> cardGO;

    bool roomSetUped = false;
    public void ResetRoom()
    {
        if (!roomSetUped && !isHall)
        {
            if (availableCards < 3)
            {
                for (int i = availableCards; i < 3; i++)
                {
                    cardGO[i].SetActive(false);
                }
            }
            if (availablePlaces < 3)
            {
                for (int i = availablePlaces; i < 3; i++)
                {
                    placGO[i].SetActive(false);
                }
            }
            roomSetUped = true;
        }

        placeTaken = 0;
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i] = -1;
        }
        UpdateRoom();
    }

    public void RollCards()
    {
        for (int i = 0; i < availableCards; i++)
        {
            cardList.Add(Boardmanager.instance.RollItem());
        }
        UpdateRoom();
    }

    public bool TakePlace()
    {
        if (placeTaken < availablePlaces)
        {
            placeTaken++;
            return true;
        }
        else
            return false;
    }

    public void UpdateRoom()
    {
        if (!isHall)
        {
            for (int i = 0; i < availableCards; i++)
            {
                ItemManager item = cardGO[i].GetComponentInChildren<ItemManager>();
                if (cardList.Count > i)
                    item.SetItem(cardList[i]);
                else
                {
                    item.SetItem(-1);
                }
            }
        }
        else
        {
            ItemManager item = cardGO[0].GetComponentInChildren<ItemManager>();
            item.SetItemofType((int)ItemType.QuestionMark);
        }
    }

}

