using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayersHolder : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

    public void ResetPlayer()
    {
        m_score = 0;
        m_greeenCardsSell = 0;
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
        Debug.Log("GetCardValue");
        int cardValue = 100;

        RemoveCardID(person, id);
        m_score += cardValue;
        UpdatePlayer();
    }

    void NeedToRemoveCard (List<int> cards, Person person)
    {
        Debug.Log("Show menu Remove card");
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


