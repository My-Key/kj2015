using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHolder
{
    public bool m_isPlayer = false;

    public int m_score = 0;
    public int m_greeenCardsSell = 0;
    public Text m_scoreText;

    public List<int> childCards = new List<int>();
    public List<int> ladyCards = new List<int>();
    public List<int> manCards = new List<int>();

    public int m_childRoomPick = -1;
    public int m_ladyRoomPick = -1;
    public int m_manRoomPick = -1;

    public List<GameObject> PawnGo;
    public List<Vector3> PawnStartPos;

    bool playerSetup = false;

    public List<GameObject> childCardGO;
    public List<GameObject> ladyCardGO;
    public List<GameObject> manCardGO;

    public void ResetPlayer()
    {
        Debug.Log("ResetPlayer");
        m_score = 0;
        m_greeenCardsSell = 0;
        childCards = new List<int>();
        ladyCards = new List<int>();
        manCards = new List<int>();

        m_childRoomPick = -1;
        m_ladyRoomPick = -1;
        m_manRoomPick = -1;

        if (!playerSetup)
        {
            for (int i = 0; i < PawnGo.Count; i++)
			{
                PawnStartPos.Add(PawnGo[i].transform.position);
			}
            
            playerSetup = true;
        }

        UpdatePlayer();
    }

    public void AddCard(Person person, int card)
    {
        if (card == -1)
            return;
        switch (person)
        {
            case Person.Child:
                AddChildCard(card);
                break;
            case Person.Lady:
                AddLadyCard(card);
                break;
            case Person.Man:
                AddManCard(card);
                break;
        }
        UpdatePlayer();
    }


    void AddChildCard(int card)
    {
        Debug.Log("AddChildCard" + card);
        childCards.Add(card);
        if (childCards.Count > 1)
        {
            NeedToRemoveCard(childCards, Person.Child);
        }
        Debug.Log("AddChildCard" + card + "|" + childCards.Count);
    }

    void AddLadyCard(int card)
    {
        ladyCards.Add(card);
        if (ladyCards.Count > 2)
        {
            NeedToRemoveCard(ladyCards, Person.Lady);
        }
    }

    void AddManCard(int card)
    {
        manCards.Add(card);
        if (manCards.Count > 3)
        {
            NeedToRemoveCard(manCards, Person.Man);
        }
    }

    public void ProcessSellCard()
    {

        int id = 0;

        if (childCards.Contains(id))
        {
            SellCard(id, Person.Child);
        }
        if (ladyCards.Contains(id))
        {
            SellCard(id, Person.Lady);
        }
        if (manCards.Contains(id))
        {
            SellCard(id, Person.Man);
        }
    }


    public void SellCard(int id, Person person)
    {
        int cardValue = GetCurrentCardValue(id);

        RemoveCardID(person, id);
        m_score += cardValue;
        UpdatePlayer();
    }
    public void ForceSellCard(int id, Person person)
    {
        int cardValue = -GetCurrentCardValue(id);

        RemoveCardID(person, id);
        m_score += cardValue;
        UpdatePlayer();
    }

    int GetCurrentCardValue(int id)
    {
        return Boardmanager.instance.GetItemData(id).price;
    }

    void NeedToRemoveCard(List<int> cards, Person person)
    {
        if (m_isPlayer)
		{
			Boardmanager.instance.m_DiscardPanel.Show (false, person, cards);
		}
        else
        {
            int lowPice = 0;
            int cardID = 0;
            bool firstSell = true;
            List<int> cardstmp = new List<int>();
            switch (person)
            {
                case Person.Child:
                    cards = childCards;
                    break;
                case Person.Lady:
                    cards = ladyCards;
                    break;
                case Person.Man:
                    cards = manCards;
                    break;
            }

            for (int i = 0; i < cardstmp.Count; i++)
            {
                if (firstSell)
                {
                    cardID = cardstmp[i];
                    lowPice = GetCurrentCardValue(cardID);
                    firstSell = false;
                }
                else if (GetCurrentCardValue(cardstmp[i]) < lowPice)
                {
                    cardID = cardstmp[i];
                    lowPice = GetCurrentCardValue(cardID);
                }
            }

            RemoveCardID(person, cardID);
        }


    }

    public void RemoveCardPlace(Person person, int cardPlace)
    {
        switch (person)
        {
            case Person.Child:
                childCards.RemoveAt(cardPlace);
                break;
            case Person.Lady:
                ladyCards.RemoveAt(cardPlace);
                break;
            case Person.Man:
                manCards.RemoveAt(cardPlace);
                break;
        }
        UpdatePlayer();
    }

    public void RemoveCardID(Person person, int id)
    {
        Boardmanager.instance.GiveBackCard(id);
        switch (person)
        {
            case Person.Child:
                childCards.Remove(id);
                break;
            case Person.Lady:
                ladyCards.Remove(id);
                break;
            case Person.Man:
                manCards.Remove(id);
                break;
        }
        UpdatePlayer();
    }

    //childCardGO;  ladyCardGO;     manCardGO
    //childCards    ladyCards   manCards
    public void UpdatePlayer()
    {
        if (m_isPlayer)
        {
            for (int i = 0; i < 1; i++)
            {
                ItemManager item = childCardGO[i].GetComponentInChildren<ItemManager>();
                if (childCards.Count > i)
                    item.SetItem(childCards[i]);
                else
                {
                    item.SetItem(-1);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                ItemManager item = ladyCardGO[i].GetComponentInChildren<ItemManager>();
                if (ladyCards.Count > i)
                    item.SetItem(ladyCards[i]);
                else
                {
                    item.SetItem(-1);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                ItemManager item = manCardGO[i].GetComponentInChildren<ItemManager>();
                if (manCards.Count > i)
                    item.SetItem(manCards[i]);
                else
                {
                    item.SetItem(-1);
                }
            }
        }

        if (m_score < 0)
            m_score = 0;
        m_scoreText.text = m_score + " $";

    }



}
public enum Person
{
    Child = 1,
    Lady = 2,
    Man = 3
}