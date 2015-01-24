using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

    public float m_liveTime = 3f;
    float m_time = 0;

    bool isAlive = false;
	// Use this for initialization
	void Start () 
    {
        isAlive = true;
        m_time = m_liveTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isAlive)
            return;

        m_time -= Time.deltaTime;
        if (m_time <= 0)
        {
            isAlive = false;
            PoolManager.Destroy(this.gameObject);
        }
	}
}
