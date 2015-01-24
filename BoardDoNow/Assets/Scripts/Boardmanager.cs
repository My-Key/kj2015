using UnityEngine;
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
            int newItem = Random.Range(0, items.Count);
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

    public void StartGame()
    {

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

            m_RollPanel.Show(false);
        }
    }


    public void RollSeen()
    {
        m_RollPanel.Hide(true);
        StartCoroutine(WaitAndShowRoomPanel(0, 3f));
    }




    public void PickRoom()
    {
        Debug.Log("PickRoom");
    }

    public void PickRoomChild(int room)
    {
        Debug.Log("PickRoomChild " + room);
        m_ListOfPlayers[0].m_childRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(1, 1f));
    }
    public void PickRoomLady(int room)
    {
        m_ListOfPlayers[0].m_ladyRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(2, 1f));
    }
    public void PickRoomMan(int room)
    {
        m_ListOfPlayers[0].m_manRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(3, 1f));
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
                playerStart = i;
		}
        ProcesMovingFromPlayer(playerStart);
    }

    public void ProcesMovingFromPlayer(int playerStart)
    {
        for (int j = 0; j < m_ListOfPlayers.Count; j++)
        {
            for (int i = 0; i < m_ListOfPlayers.Count; i++)
            {
                int playerIndex = i + playerStart;
                if (playerIndex >= m_ListOfPlayers.Count)
                    playerIndex -= m_ListOfPlayers.Count;
                MovePawnPlayer(playerIndex, j);
            }
        }
        
    }

    public void MovePawnPlayer(int playerIndex, int type)
    {
        
        StartCoroutine(MovePawn(m_ListOfPlayers[playerIndex].PawnGo[type], type, playerIndex));
    }


    IEnumerator MovePawn(GameObject pawn, int type, int player)
    {
        int room = 0;
        switch (type)
        {
	        case (int)Person.Child:
                room = m_ListOfPlayers[player].m_childRoomPick;
                break;
            case (int)Person.Lady:
                room = m_ListOfPlayers[player].m_ladyRoomPick;
                break;
            case (int)Person.Man:
                room = m_ListOfPlayers[player].m_manRoomPick;
                break;
        }
        int place = m_RoomsList[room].TakePlace();
        if (place >=0)
        {
            pawn.transform.DOMove(m_RoomsList[room].placGO[place].transform.position, 2f);
            yield return new WaitForSeconds(2f);
            m_ListOfPlayers[player].AddCard((Person)type, m_RoomsList[room].TakeCard());
        }
        else
        {
            pawn.transform.DOMove(m_RoomsList[room].specialPlace.transform.position, 2f);
        }
    }
}
