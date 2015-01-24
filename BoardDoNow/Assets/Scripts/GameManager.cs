using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour 
{

    public Image m_FadeBlack;

    public Panel m_StartGamePanel;
    public GameObject m_Manor;

    public GameObject m_Board;

    public List<GameObject> objectsToDisableOnStart;
    

    GameState gamestate = GameState.Start;
    public static GameManager instance;

    
	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < objectsToDisableOnStart.Count; i++)
        {
            objectsToDisableOnStart[i].SetActive(false);
        }
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        instance = this;
        ShowStartGame();

        
  	}
	
	// Update is called once per frame
	/*void Update () 
    {

	}*/


    void ShowStartGame()
    {
        gamestate = GameState.Start;
        m_StartGamePanel.Show(true);
        m_Manor.SetActive(true);
    }


    void ResetGame()
    {
        gamestate = GameState.Start;
    }





    public void QuitGame()
    {
        Application.Quit();
    }


    public void StartGame()
    {
        m_StartGamePanel.Hide(false);
        StartCoroutine(InitializeGame());
    }



    IEnumerator InitializeGame()
    {
        Fade(1, 2);
        yield return new WaitForSeconds(2f);
        m_Manor.SetActive(false);
        m_Board.gameObject.SetActive(true);
        //GameManager.instance.Fade(0f, 2f);
        yield return new WaitForSeconds(1f);
        Boardmanager.instance.StartGame();
    }



    public void MenuButton()
    {

    }





    public void Fade(float value, float time)
    {
        m_FadeBlack.DOFade(value, time);
    }












    enum GameState
    {
        Start,
        Menu,
        Playing,
        Paused
    }
}
