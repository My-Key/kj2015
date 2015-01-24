using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Boardmanager : MonoBehaviour {

    public static Boardmanager instance;
    public int greenDice = 0;
    public int redDice = 0;
    public int currentTurn = 0;
    public int maxTurns = 10;

    public GameObject diceGreenOBj;
    public GameObject diceRedOBj;

    public Text m_TurnText;

    public List<Item> items;
    public List<PlayerHolder> m_ListOfPlayers;

    public Panel m_RollPanel;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public void RollDices()
    {
        greenDice = Random.Range(1, 6);
        redDice = greenDice;
        while (redDice == greenDice)
        {
            redDice = Random.Range(1, 6);
        }

        ItemManager greenDiceim = diceGreenOBj.GetComponentInChildren<ItemManager>();
        ItemManager redDiceim = diceRedOBj.GetComponentInChildren<ItemManager>();

        greenDiceim.SetItemofType(greenDice);
        redDiceim.SetItemofType(redDice);
        
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

    public Item GetItemData(int id)
    {
        return items[id];
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
        m_RollPanel.Hide(true);
    }

    public void StartTurn()
    {
        currentTurn++;

        if (currentTurn > maxTurns)
        {
            Debug.Log("ProcessEndGame");
        }
        else
        {
            m_TurnText.text = currentTurn + " / " + maxTurns; 
            RollDices();
            m_RollPanel.Show(false);
        }
    }


    public void StartGame()
    {
        
        currentTurn = 0;
        ResetItems();
        for (int i = 0; i < m_ListOfPlayers.Count; i++)
        {
            m_ListOfPlayers[i].ResetPlayer();
        }
        StartTurn();
    }


    

}
