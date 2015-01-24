using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour 
{

    public string m_SceneToLoad = "Level1";
    public GameObject m_PlayerPrefab;
    public GameObject m_PlayerCameraPrefab;
    public GameObject m_GUIPrefab;


    private GameObject m_player;
    private GameObject m_playerCamera;
    private GameObject m_gui;

    public GameObject mainSpawner;

    bool firstSpawn = true;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        Application.LoadLevel(m_SceneToLoad);
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
            m_player.transform.position = mainSpawner.transform.position;
        }
        
    }
	
}
