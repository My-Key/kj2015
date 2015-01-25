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
	public DiscardPanel m_DiscardPanel;

    public GameObject cammerdinerGo;
    public Vector3 cammedrinerStart;
    public int cammerinderRoom = 5;

    public EndPanel m_EndPanel;

	public static bool waitForEndOfDiscard = false;

    public int[,] moveTable;

    public int cammerdinerMoves = 1;

	public List<GameObject> layers;


    public List<AudioClip> starTurSound;
    public List<AudioClip> clickSound;
    public List<AudioClip> cashSound;
    public List<AudioClip> catchSound;
    public List<AudioClip> shufleSound;
    public List<AudioClip> winSound;
    public List<AudioClip> loseSound;
    public List<AudioClip> walkSound;
    public List<AudioClip> walkButlerSound;
    public List<AudioClip> itemCollectSound;

    
    // Use this for initialization
    void Start()
    {
        instance = this;
        SetUpMoveBoard();
    }

    public void RollDices()
    {
        greenDice = Random.Range(0, 5);
        redDice = greenDice;
        while (redDice == greenDice)
        {
            redDice = Random.Range(0, 5);
        }

        ItemManager greenDiceim = diceGreenOBj.GetComponentInChildren<ItemManager>();
        ItemManager redDiceim = diceRedOBj.GetComponentInChildren<ItemManager>();

        greenDiceim.ShufleAnimation(greenDice);
        redDiceim.ShufleAnimation(redDice);

        
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
            int newItem = Random.Range(51, 66);
            if (items[newItem].available)
            {
                items[newItem].available = false;
                item = newItem;
            }

        }
        return item;
    }

    public void GiveBackCard(int id)
    {
        if (id >= 0 && id < items.Count)
        {
            items[id].available = true;
        }
    }
    

    public Item GetItemData(int id)
    {
        if (id >= 0)
            return items[id];
        else
            return null;
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
        SpawnSound(starTurSound);
        cammerdinerMoves = 1;
        currentTurn++;

        if (currentTurn > maxTurns)
        {
            bool didPlayerWin =DidPlayerWin();
            if (didPlayerWin)
                SpawnSound(winSound);
            else
                SpawnSound(loseSound);

            m_EndPanel.Show(false, didPlayerWin, m_ListOfPlayers[0].m_score);

        }
        else
        {
            m_TurnText.text = currentTurn + "/" + maxTurns; 
            RollDices();

            for (int i = 0; i < m_RoomsList.Count; i++)
            {
                m_RoomsList[i].RollCards();
            }

            m_RollPanel.Show(true);
        }
    }

    public bool DidPlayerWin()
    {
        int playerScore = m_ListOfPlayers[0].m_score;
        int maxEnemiesScore = 0;

        for (int i = 1; i < m_ListOfPlayers.Count; i++)
        {
            maxEnemiesScore = Mathf.Max(maxEnemiesScore, m_ListOfPlayers[i].m_score);
        }

        if (playerScore == maxEnemiesScore)
        {
            playerScore = m_ListOfPlayers[0].m_greeenCardsSell;
            maxEnemiesScore = 0;
            for (int i = 1; i < m_ListOfPlayers.Count; i++)
            {
                maxEnemiesScore = Mathf.Max(maxEnemiesScore, m_ListOfPlayers[i].m_greeenCardsSell);
            }

            return playerScore >= maxEnemiesScore;
        }

        return playerScore > maxEnemiesScore;
    }




    public void RollSeen()
    {
        m_RollPanel.Hide(true);
        StartCoroutine(WaitAndShowRoomPanel(0, 3f));
    }


    public void PickRoomChild(int room)
    {
        SpawnSound(clickSound);
        m_ListOfPlayers[0].m_childRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(1, 0.5f));
    }
    public void PickRoomLady(int room)
    {
        SpawnSound(clickSound);
        m_ListOfPlayers[0].m_ladyRoomPick = room;
        StartCoroutine(WaitAndShowRoomPanel(2, 0.5f));
    }
    public void PickRoomMan(int room)
    {
        SpawnSound(clickSound);
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

        ProcessAI1();
        ProcessAI2();

        for (int i = 3; i < m_ListOfPlayers.Count; i++)
        {
            m_ListOfPlayers[i].m_childRoomPick = Random.Range(0, 5);
            m_ListOfPlayers[i].m_ladyRoomPick = Random.Range(0, 5);
            m_ListOfPlayers[i].m_manRoomPick = Random.Range(0, 5);
        }

        ProcessMovePionkow();
    }



    void ProcessAI1()
    {
        List<int> roomValue = new List<int>();
        for (int i = 0; i < m_RoomsList.Count - 1; i++)
        {
            int tmpRoomValue = 0;
            for (int j = 0; j < m_RoomsList[i].cardList.Count; j++)
            {
                tmpRoomValue += GetItemData(m_RoomsList[i].cardList[j]).price;
            }
            roomValue.Add(tmpRoomValue);
        }
        List<int> roomSort = roomValue;
        roomSort.Sort();


        /*for (int i = 0; i < roomSort.Count; i++)
        {
            Debug.LogWarning("AI1 " + i + "|" + roomSort[i]);
        }*/

        int indexOfRoom = roomValue.IndexOf(roomSort[roomSort.Count - 1]);
        m_ListOfPlayers[1].m_childRoomPick = indexOfRoom;
        if (m_RoomsList[indexOfRoom].availablePlaces > 1)
        {
            m_ListOfPlayers[1].m_ladyRoomPick = indexOfRoom;
            m_ListOfPlayers[1].m_manRoomPick = roomValue.IndexOf(roomSort[roomSort.Count - 2]);
        }
        else
        {
            m_ListOfPlayers[1].m_ladyRoomPick = roomValue.IndexOf(roomSort[roomSort.Count - 2]);
            m_ListOfPlayers[1].m_manRoomPick = roomValue.IndexOf(roomSort[roomSort.Count - 3]);
        }

    }



    void ProcessAI2()
    {
        List<int> itemValue = new List<int>();
        for (int i = 0; i < m_RoomsList.Count - 1; i++)
        {
            for (int j = 0; j < m_RoomsList[i].cardList.Count; j++)
            {
                itemValue.Add( GetItemData(m_RoomsList[i].cardList[j]).price);
            }
        }
        itemValue.Sort();

        /*for (int i = 0; i < itemValue.Count; i++)
        {
            Debug.LogWarning("AI2 " + i + "|" + itemValue[i]);
        }*/

        for (int i = 0; i < m_RoomsList.Count - 1; i++)
        {
            for (int j = 0; j < m_RoomsList[i].cardList.Count; j++)
            {
                if (GetItemData(m_RoomsList[i].cardList[j]).price == itemValue[itemValue.Count - 1])
                {
                    m_ListOfPlayers[2].m_childRoomPick = i;
                }
            }
        }

        for (int i = 0; i < m_RoomsList.Count - 1; i++)
        {
            for (int j = 0; j < m_RoomsList[i].cardList.Count; j++)
            {
                if (GetItemData(m_RoomsList[i].cardList[j]).price == itemValue[itemValue.Count - 2])
                {
                    m_ListOfPlayers[2].m_ladyRoomPick = i;
                }
            }
        }

        for (int i = 0; i < m_RoomsList.Count; i++)
        {
            for (int j = 0; j < m_RoomsList[i].cardList.Count; j++)
            {
                if (GetItemData(m_RoomsList[i].cardList[j]).price == itemValue[itemValue.Count - 3])
                {
                    m_ListOfPlayers[2].m_manRoomPick = i;
                }
            }
        }

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
				while (waitForEndOfDiscard)
					yield return new WaitForSeconds(0.5f);

                int playerIndex = i + playerStart;
                if (playerIndex >= m_ListOfPlayers.Count)
                    playerIndex -= m_ListOfPlayers.Count;
                int type = j;
                
                GameObject pawn = m_ListOfPlayers[playerIndex].PawnGo[type];

                int room = 0;
                switch (type)
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
					SetParentByPosition(m_RoomsList[room].placGO[place].transform.position, pawn);
                    SpawnSound(walkSound);
                    yield return new WaitForSeconds(2f);
                    SpawnSound(itemCollectSound);
                    m_ListOfPlayers[playerIndex].AddCard((Person)(type), m_RoomsList[room].TakeCard());
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    place = m_RoomsList[5].TakePlace();
                    pawn.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 2f);
                    pawn.transform.DOMove(m_RoomsList[room].specialPlace.transform.position, 2f);
                    SpawnSound(walkSound);
                    yield return new WaitForSeconds(2f);

					pawn.transform.DOMove(m_RoomsList[5].placGO[place].transform.position, 2f);
					SetParentByPosition(m_RoomsList[5].placGO[place].transform.position, pawn);
                    SpawnSound(walkSound);
					yield return new WaitForSeconds(2f);

                    switch (type)
                    {
	                    case (int)Person.Child:
                            m_ListOfPlayers[playerIndex].m_childRoomPick = 5;
                            break;
                        case (int)Person.Lady:
                            m_ListOfPlayers[playerIndex].m_ladyRoomPick = 5;
                            break;
                        case (int)Person.Man:
                            m_ListOfPlayers[playerIndex].m_manRoomPick = 5;
                            break;
                    }
                    SpawnSound(itemCollectSound);
                    int item = RollCrapyItem();
                    if (cammerdinerMoves == 1 && item >63)
                        cammerdinerMoves++;
                    m_ListOfPlayers[playerIndex].AddCard((Person)(type), item);
                }
            }
        }
        ProcessCammerdiner();
        
    }

	public void SetParentByPosition(Vector3 position, GameObject obj)
	{
		GameObject newParent = null;

		foreach (GameObject parent in layers)
		{
			if (newParent == null || (Mathf.Abs(parent.transform.position.y - position.y) < Mathf.Abs(newParent.transform.position.y - position.y)))
			{
				newParent = parent;
			}
		}

		if (newParent != null)
			obj.transform.parent = newParent.transform;
	}

    void ProcessCammerdiner()
    {
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
            room = moveTable[cammerinderRoom, room];

            StartCoroutine(MoveCammerdiner(room));
        }
        

        
    }



    IEnumerator MoveCammerdiner(int room)
    {
		cammerdinerGo.transform.DOMove(m_RoomsList[room].specialPlace.transform.position, 2f);
		SetParentByPosition(m_RoomsList[room].specialPlace.transform.position, cammerdinerGo);
		cammerdinerGo.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 2f);
        cammerinderRoom = room;
        SpawnSound(walkButlerSound);
        yield return new WaitForSeconds(3f);

        while (waitForEndOfDiscard)
            yield return new WaitForSeconds(0.5f);

        cammerdinerMoves--;
        if (cammerdinerMoves == 0)
            CheckPlayersItemsCammerdiner();
        else
        {
            ProcessCammerdiner();
        }
    }


    void CheckPlayersItemsCammerdiner()
    {
        int catched = 0;
        for (int i = 0; i < m_ListOfPlayers.Count; i++)
        {
            if (m_ListOfPlayers[i].m_childRoomPick == cammerinderRoom)
            {
                for (int a = 0; a < m_ListOfPlayers[i].childCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].childCards[a]);
                    if (redDice == (int)item.itemType)
                    {
                        Debug.Log("cammerdinerCatch " + i + " Child " + redDice + "|" + m_ListOfPlayers[i].childCards[a] + "|" + (int)item.itemType + " camroom " + cammerinderRoom + "|" + m_ListOfPlayers[i].m_childRoomPick);
                        m_ListOfPlayers[i].ForceSellCard(m_ListOfPlayers[i].childCards[a], Person.Child);
                        m_ListOfPlayers[i].PawnGo[0].transform.DOShakeRotation(2, 20);
                        cammerdinerGo.transform.DOShakeRotation(2, 20);
                        catched++;
                    }
                }
            }
            if (m_ListOfPlayers[i].m_ladyRoomPick == cammerinderRoom)
            {
                for (int a = 0; a < m_ListOfPlayers[i].ladyCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].ladyCards[a]);
                    if (redDice == (int)item.itemType)
                    {
                        Debug.Log("cammerdinerCatch " + i + " Lady " + redDice + "|" + m_ListOfPlayers[i].ladyCards[a] + "|" + (int)item.itemType + " camroom " + cammerinderRoom + "|" + m_ListOfPlayers[i].m_ladyRoomPick);
                        m_ListOfPlayers[i].ForceSellCard(m_ListOfPlayers[i].ladyCards[a], Person.Lady);
                        m_ListOfPlayers[i].PawnGo[1].transform.DOShakeRotation(2, 20);
                        cammerdinerGo.transform.DOShakeRotation(2, 20);
                        catched++;
                    }
                }
            }
            if (m_ListOfPlayers[i].m_manRoomPick == cammerinderRoom)
            {
                for (int a = 0; a < m_ListOfPlayers[i].manCards.Count; a++)
                {
                    Item item = GetItemData(m_ListOfPlayers[i].manCards[a]);
                    if (redDice == (int)item.itemType)
                    {
                        Debug.Log("cammerdinerCatch " + i + " Man " + redDice + "|" + m_ListOfPlayers[i].manCards[a] + "|" + (int)item.itemType + " camroom " + cammerinderRoom + "|" + m_ListOfPlayers[i].m_manRoomPick);
                        m_ListOfPlayers[i].ForceSellCard(m_ListOfPlayers[i].manCards[a], Person.Man);
                        m_ListOfPlayers[i].PawnGo[2].transform.DOShakeRotation(2, 20);
                        cammerdinerGo.transform.DOShakeRotation(2, 20);
                        catched++;
                    }
                }
            }
        }
        if (catched > 0)
            SpawnSound(catchSound);
        StartCoroutine(WaitAndSell());
    }

    IEnumerator WaitAndSell()
    {
        yield return new WaitForSeconds(2f);
        while (waitForEndOfDiscard)
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
        cammerdinerGo.transform.DOMove(m_RoomsList[cammerinderRoom].m_butlerPlace.transform.position, 2f);
        SetParentByPosition(m_RoomsList[cammerinderRoom].m_butlerPlace.transform.position, cammerdinerGo);

        yield return new WaitForSeconds(2f);
        CheckPlayersItems();
    }

    void CheckPlayersItems()
    {
        
        for (int i = 0; i < m_ListOfPlayers.Count; i++)
        {
            for (int a = 0; a < m_ListOfPlayers[i].childCards.Count; a++)
            {
                Item item = GetItemData(m_ListOfPlayers[i].childCards[a]);
                if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                {
                    m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].childCards[a], Person.Child);
                    m_ListOfPlayers[i].m_greeenCardsSell++;
                }
            }
            for (int a = 0; a < m_ListOfPlayers[i].ladyCards.Count; a++)
            {
                Item item = GetItemData(m_ListOfPlayers[i].ladyCards[a]);
                if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                {
                    m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].ladyCards[a], Person.Lady);
                    m_ListOfPlayers[i].m_greeenCardsSell++;
                }
            }
            for (int a = 0; a < m_ListOfPlayers[i].manCards.Count; a++)
            {
                Item item = GetItemData(m_ListOfPlayers[i].manCards[a]);
                if (greenDice == (int)item.itemType || item.itemType == ItemType.QuestionMark)
                {
                    m_ListOfPlayers[i].SellCard(m_ListOfPlayers[i].manCards[a], Person.Man);
                    m_ListOfPlayers[i].m_greeenCardsSell++;
                }
            }
        }
        SpawnSound(cashSound);
        StartCoroutine(WaitAndStartTurn());
    }

    IEnumerator WaitAndStartTurn()
    {
        while (waitForEndOfDiscard)
            yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(3f);
        StartTurn();
    }

    void SetUpMoveBoard()
    {
        moveTable = new int[6, 6] {
        {0,1,5,5,5,5},
        {0,1,0,0,0,0},
        {5,5,2,5,5,5},
        {4,4,4,3,4,4},
        {5,5,5,3,4,5},
        {0,0,2,4,4,5},
            };

    }


    public void SpawnSound(List<AudioClip> clips)
    {
        if (clips != null && clips.Count > 0)
            AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Count)], Camera.main.transform.position);
    }
}


