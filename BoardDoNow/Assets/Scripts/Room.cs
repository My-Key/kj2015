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
    public GameObject specialPlace;
    public GameObject m_butlerPlace;

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
        placeTaken = 0;

        for (int i = 0; i < cardList.Count; i++)
        {
            Boardmanager.instance.GiveBackCard(cardList[i]);
        }

        cardList = new List<int>();

        for (int i = 0; i < availableCards; i++)
        {
            cardList.Add(Boardmanager.instance.RollItem());
        }
        UpdateRoom();
    }

    public int TakePlace()
    {
        if (placeTaken < availablePlaces)
        {
            placeTaken++;
            if (isHall && placeTaken > 8)
                return placeTaken -= 8;
            else
                return placeTaken -1;
        }
        else
            return -1;
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

    public int TakeCard()
    {
        if (cardList.Count > 0)
        {
            int cardToReturn = cardList[0];
            cardList.RemoveAt(0);
            UpdateRoom();
            return cardToReturn;
        }
        else
            return -1;
    }

}

