using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class GameLoader : MonoBehaviour 
{
    [HideInInspector]
    public static GameLoader instance;

    public string m_SceneToLoad = "Level1";
    public GameObject m_PlayerPrefab;
    public GameObject m_PlayerCameraPrefab;
    public GameObject m_GUIPrefab;

    public Image m_Loading;
    public Text m_LoadingText;

    private GameObject m_player;
    private GameObject m_playerCamera;
    private GameObject m_gui;

    public GameObject mainSpawner;

    public Transform lastSavePoint;

    bool firstSpawn = true;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Application.LoadLevel(m_SceneToLoad);
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
	}

    void OnLevelWasLoaded(int level)
    {

        if (Application.loadedLevelName != "Loading")
        {
            mainSpawner = GameObject.Find("PlayerSpawner");
        }
        if (firstSpawn)
        {
            m_gui = PoolManager.Instantiate(m_GUIPrefab);
            m_player = PoolManager.Instantiate(m_PlayerPrefab, mainSpawner.transform.position, Quaternion.identity);
            m_playerCamera = PoolManager.Instantiate(m_PlayerCameraPrefab, mainSpawner.transform.position + new Vector3(0, 0, -10), Quaternion.identity);
            m_playerCamera.GetComponent<CameraFollow>().player = m_player.transform;
            DontDestroyOnLoad(m_gui);
            DontDestroyOnLoad(m_player);
            DontDestroyOnLoad(m_playerCamera);
            firstSpawn = false;
            
        }
        else
        {
            BackgroundParallax.instance.SetParallax(m_playerCamera.GetComponentInChildren<Camera>().transform);
            m_player.transform.position = mainSpawner.transform.position;
        }
        lastSavePoint = mainSpawner.transform;
        LoadingFadeOut();
    }
	

    void LoadingFadeOut()
    {
        m_Loading.DOFade(0, 2f);
        m_LoadingText.DOFade(0, 2f);
    }

    void LoadingFadeIn()
    {
        m_Loading.DOFade(1, 2f);
        m_LoadingText.DOFade(1, 2f);
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelEnum(levelName));
    }



    IEnumerator LoadLevelEnum (string levelName)
    {
        LoadingFadeIn();
        yield return new WaitForSeconds(2f);
        Application.LoadLevel(levelName);
    }


    public void ResetPlayer()
    {

    }

    IEnumerator AnimationResetPlayer()
    {
        LoadingFadeIn();
        yield return new WaitForSeconds(2f);

        m_player.transform.position = lastSavePoint.transform.position;
    }
}
