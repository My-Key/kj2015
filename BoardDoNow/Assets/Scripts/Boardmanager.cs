﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class Boardmanager : MonoBehaviour {

    public static Boardmanager instance;
    public int greenDice = 0;
    public int redDice = 0;
    public int currentTurn = 0;
    public int maxTurns = 7;

    public GameObject diceGreenOBj;
    public GameObject diceRedOBj;

    public List<GameObject> m_RoomPicker;

    public Text m_TurnText;

    public List<Item> items;
    public List<PlayerHolder> m_ListOfPlayers;
    public List<Room> m_RoomsList;

    public Panel m_RollPanel;

    public GameObject cammerdinerGo;
    public Vector3 cammedrinerStart;
    public int cammerinderRoom = 5;

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
            int newItem = Random.Range(0, 50);
            if (items[newItem].available)
            {
                items[newItem].available = false;
                item = newItem;
            }

        }
        return item;
    }

    public int RollCrapyItem()
    {
        int item = -1;
        while(item == -1)
        {
            int newItem = Random.Range(51, items.Count-50);
            if (items[newItem].available)
            {
                items[newItem].available = false;
                item = newItem;
            }

        }
        Debug.Log("RollCrapyItem " + item);
        return item;
    }

    public void GiveBackCard(int id)
    {
        Debug.Log("GiveBackCard " + id + " available " + items[id].available);
        items[id].available = true;
    }
    

    public Item GetItemData(int id)
    {
        return items[id];
    }

    public void StartGame()
    {
        if (cammedrinerStart == Vector3.zero)
            cammedrinerStart = cammerdinerGo.transform.position;
        cammerinderRoom = 5;
        currentTurn = 0;
        ResetItems();
        ResetRooms();
        
        for (int i = 0; i < m_ListOfPlayers.Count; i++)
        {
            m_ListOfPlayers[i].ResetPlayer();
        }
        StartTurn();
    }

    public void ResetItems()
    {
        for (int i = 0; i < items.Count; i++)
		{
			 items[i].available = true;
		}
    }

    public void ResetRooms()
    {
        for (int i = 0; i < m_RoomsList.Count; i++)
        {
            m_RoomsList[i].ResetRoom();
        }
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
            Debug.Log("Start Turn: " + currentTurn + "/" + maxTurns);
            m_TurnText.text = currentTurn + "/" + maxTurns; 
            RollDices();

            for (int i = 0; i < m_RoomsList.Count; i++)
            {
                m_RoomsList[i].RollCards();
            }

            m_RollPanel.Show(true);
        }
    }


    public void RollSeen()
    {
        m_RollPanel.Hide(true);
        StartCoroutine(WaitAndShowRoomPanel(0, 3f));
    }


    public void PickRoomChild(int room)
    {
        m_ListOfPlayers[0].m_childRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(1, 0.5f));
    }
    public void PickRoomLady(int room)
    {
        m_ListOfPlayers[0].m_ladyRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(2, 0.5f));
    }
    public void PickRoomMan(int room)
    {
        m_ListOfPlayers[0].m_manRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(3, 0.5f));
    }
    

    IEnumerator WaitAndShowRoomPanel(int panel, float timeTowait)
    {
        if (panel > 0)
            m_RoomPicker[panel-1].SetActive(false);
        yield return new WaitForSeconds(timeTowait);
        if (panel < 3)
            m_RoomPicker[panel].SetActive(true);
        else
            ProcesAIMoves();
    }


    public void ProcesAIMoves()
    {
        Debug.Log("ProcesAIMoves");

        for (int i = 1; i < m_ListOfPlayers.Count; i++)
        {
            m_ListOfPlayers[i].m_childRoomPick = Random.Range(0, 5);
            m_ListOfPlayers[i].m_ladyRoomPick = Random.Range(0, 5);
            m_ListOfPlayers[i].m_manRoomPick = Random.Range(0, 5);
        }

        ProcessMovePionkow();
    }

    public void ProcessMovePionkow()
    {
        int minRoomNumber = 6;
        int playerStart = -1;
        for (int i = 0; i < m_ListOfPlayers.Count; i++)
		{
            if (m_ListOfPlayers[i].m_childRoomPick < minRoomNumber)
            {
                playerStart = i;
                minRoomNumber = m_ListOfPlayers[i].m_childRoomPick;
            }
		}
        ProcesMovingFromPlayer(playerStart);
    }

    public void ProcesMovingFromPlayer(int playerStart)
    {
        StartCoroutine(MovePawn(playerStart));
        
    }


    IEnumerator MovePawn( int playerStart)
    {
        
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < m_ListOfPlayers.Count; i++)
            {
                int playerIndex = i + playerStart;
                if (playerIndex >= m_ListOfPlayers.Count)
                    playerIndex -= m_ListOfPlayers.Count;
                int type = j;
                
                GameObject pawn = m_ListOfPlayers[playerIndex].PawnGo[type];

                int room = 0;
                switch (type + 1)
                {
	                case (int)Person.Child:
                        room = m_ListOfPlayers[playerIndex].m_childRoomPick;
                        break;
                    case (int)Person.Lady:
                        room = m_ListOfPlayers[playerIndex].m_ladyRoomPick;
                        break;
                    case (int)Person.Man:
                        room = m_ListOfPlayers[playerIndex].m_manRoomPick;
                        break;
                }

                int place = m_RoomsList[room].TakePlace();
                if (place >=0)
                {
                    pawn.transform.DOMove(m_RoomsList[room].placGO[place].transform.position, 2f);
                    pawn.transform.DOScale(new Vector3(0.5f,0.5f,0.5f), 2f);
                    yield return new WaitForSeconds(2f);
                    m_ListOfPlayers[playerIndex].AddCard((Person)(type+1), m_RoomsList[room].TakeCard());
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    place = m_RoomsList[5].TakePlace();
                    pawn.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
                    pawn.transform.DOMove(m_RoomsList[room].specialPlace.transform.position, 2f);
                    yield return new WaitForSeconds(2f);
                    pawn.transform.DOMove(m_RoomsList[5].placGO[place].transform.position, 2f);
                    yield return new WaitForSeconds(1f);
                    m_ListOfPlayers[playerIndex].AddCard((Person)(type + 1), RollCrapyItem());
                }
            }
        }
        ProcessCammerdiner();
        
    }

    void ProcessCammerdiner()
    {
        Debug.Log("ProcessCammerdiner");
        if (currentTurn == 1)
        {
            StartCoroutine(MoveCammerdiner(5));
        }
        else
        {
            int maxPlayercount = 0;
            int room = 0;

            for (int i = 0; i < m_RoomsList.Count - 1; i++)
            {
                if (m_RoomsList[i].placeTaken > maxPlayercount)
                {
                    maxPlayercount = m_RoomsList[i].placeTaken;
                    room = i;
                }
            }
            StartCoroutine(MoveCammerdiner(room));
        }
        

        
    }



    IEnumerator MoveCammerdiner(int room)
    {
        Debug.Log("MoveCammerdiner");
        cammerdinerGo.transform.DOMove(m_RoomsList[room].specialPlace.transform.position, 2f);
        cammerdinerGo.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 2f);
        cammerinderRoom = room;
        yield return new WaitForSeconds(3f);
        CheckPlayersItemsCammerdiner();
    }


    void CheckPlayersItemsCammerdiner()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < m_ListOfPlayers.Count; i++)
            {
                for (int a = 0; a < m_ListOfPlayers[i].childCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].childCards[a]);
                    if (redDice == (int)item.itemType)
                    {
                        m_ListOfPlayers[i].ForceSellCard(m_ListOfPlayers[i].childCards[a], Person.Child);
                    }
                }
            }
        }
        Debug.Log("CheckPlayersItemsCammerdiner ended");
        StartCoroutine(WaitAndSell());
    }

    IEnumerator WaitAndSell()
    {
        yield return new WaitForSeconds(1f);

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < m_ListOfPlayers.Count; i++)
            {
                GameObject pawn = m_ListOfPlayers[i].PawnGo[j];
                pawn.transform.DOMove(m_ListOfPlayers[i].PawnStartPos[j], 2f);
                pawn.transform.DOScale(new Vector3(1f, 1f, 1f), 2f);

            }
        }
        yield return new WaitForSeconds(4f);
        CheckPlayersItems();
    }

    void CheckPlayersItems()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < m_ListOfPlayers.Count; i++)
            {
                for (int a = 0; a < m_ListOfPlayers[i].childCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].childCards[a]);
                    if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                    {
                        m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].childCards[a], Person.Child);
                    }
                }
                for (int a = 0; a < m_ListOfPlayers[i].ladyCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].ladyCards[a]);
                    if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                    {
                        m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].ladyCards[a], Person.Lady);
                    }
                }
                for (int a = 0; a < m_ListOfPlayers[i].manCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].manCards[a]);
                    if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                    {
                        m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].manCards[a], Person.Man);
                    }
                }
            }
        }
        StartCoroutine(WaitAndStartTurn());
    }

    IEnumerator WaitAndStartTurn()
    {
        yield return new WaitForSeconds(3f);
        StartTurn();
    }
}


