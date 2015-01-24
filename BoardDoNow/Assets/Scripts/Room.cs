using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts
{

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

        public void ResetRoom()
        {
            placeTaken = 0;
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i] = -1;
            }
            UpdateRoom();
        }

        public void RollCards()
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                Boardmanager.instance.RollItem();
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
            for (int i = 0; i < cardGO.Count; i++)
            {
                ItemManager item = cardGO[i].GetComponentInChildren<ItemManager>();
                if (cardList[i])
            }
        }

    }
}
