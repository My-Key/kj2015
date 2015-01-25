using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour 
{

    public Image m_FadeBlack;

    public StartResumePanel m_StartGamePanel;
    public GameObject m_Manor;

    public GameObject m_Board;

    public List<GameObject> objectsToDisableOnStart;
    

    public static GameManager instance;


    bool soundplaying = false;
    public GameObject sounOn;
    public GameObject soundOff;
    
	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < objectsToDisableOnStart.Count; i++)
        {
            objectsToDisableOnStart[i].SetActive(false);
        }
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
        instance = this;
        SoundButtonButton();
        ShowStartGame();

        
  	}
	

    void ShowStartGame()
    {
        m_StartGamePanel.Show(true);
        m_Manor.SetActive(true);
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
        yield return new WaitForSeconds(1f);
        GameManager.instance.Fade(0.8f, 1f);
        Boardmanager.instance.StartGame();
    }



    public void MenuButton()
    {
        Boardmanager.waitForEndOfDiscard = true;
        m_StartGamePanel.Show();
    }

    public void ResumeButton()
    {
        Boardmanager.waitForEndOfDiscard = false;
        m_StartGamePanel.Hide(true);
    }



    public void SoundButtonButton()
    {
        soundplaying = !soundplaying;
        if (soundplaying)
        {
            sounOn.SetActive(true);
            soundOff.SetActive(false);

            if (audio.clip != null)
            {
                audio.Play();
                audio.loop = true;
            }
        }
        else
        {
            sounOn.SetActive(false);
            soundOff.SetActive(true);
            if (audio.clip != null)
            {
                audio.Stop();
            }
        }
    }


    public void Fade(float value, float time)
    {
        m_FadeBlack.gameObject.SetActive(true);
        m_FadeBlack.DOFade(value, time);
        if (value == 0)
            StartCoroutine(TurnOffFade(time));
    }


    IEnumerator TurnOffFade(float time)
    {
        yield return new WaitForSeconds(time);
        m_FadeBlack.gameObject.SetActive(false);
    }


}
