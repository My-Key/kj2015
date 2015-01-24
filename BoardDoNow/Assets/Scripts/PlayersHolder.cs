using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayersHolder : MonoBehaviour 
{
    List<PlayerHolder> ListOfPlayers;

    public static PlayersHolder instance;


	// Use this for initialization
	void Start () 
    {
        instance = this;
	}
	
}
[System.Serializable]
class PlayerHolder
{
    public bool m_isPlayer = false;

    public int m_score = 0;
    public int m_greeenCardsSell = 0;
    public Text m_scoreText;

    List<int> childCards = new List<int>();
    List<int> ladyCards = new List<int>();
    List<int> manCards = new List<int>();

    public int m_childRoomPick = -1;
    public int m_ladyRoomPick = -1;
    public int m_manRoomPick = -1;

    public void ResetPlayer()
    {
        m_score = 0;
        m_greeenCardsSell = 0;
        childCards = new List<int>();
        ladyCards = new List<int>();
        manCards = new List<int>();

        m_childRoomPick = -1;
        m_ladyRoomPick = -1;
        m_manRoomPick = -1;

        UpdatePlayer();
    }

    public void AddCard(Person person, int card)
    {
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
    }


    void AddChildCard(int card)
    {
        childCards.Add(card);
        if (childCards.Count > 1)
        {
            NeedToRemoveCard(childCards, Person.Child);
        }
    }

    void AddLadyCard(int card)
    {
        ladyCards.Add(card);
        if (ladyCards.Count > 2)
        {
            NeedToRemoveCard(childCards, Person.Lady);
        }
    }

    void AddManCard(int card)
    {
        manCards.Add(card);
        if (manCards.Count > 3)
        {
            NeedToRemoveCard(childCards, Person.Man);
        }
    }

    public void ProcessSellCard()
    {
        Debug.Log("ProcessSellCard");

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

    int GetCurrentCardValue(int id)
    {
        Debug.Log("GetCardValue");
        return 100;
    }

    void NeedToRemoveCard (List<int> cards, Person person)
    {
        if (m_isPlayer)
            Debug.Log("Show menu Remove card");
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

            for (int i  = 0; i  <cardstmp.Count; i ++)
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


    public void UpdatePlayer()
    {
        m_scoreText.text = m_score.ToString();
    }

    
    
}
public enum Person
{
    Child = 1,
    Lady = 2,
    Man = 3
}


