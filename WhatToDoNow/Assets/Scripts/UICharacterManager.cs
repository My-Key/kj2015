using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharacterManager : MonoBehaviour
{
    public Text m_hpText;
    public Image m_Heart;
    public RectTransform healthBar;

    public float m_MinColorValue = 0.8f;

    float currentHp = 1f;

    public float m_MaxBlinkTime = 0.8f;
    public float m_MinBlinkTime = 0.2f;

    float m_currentTime = 0f;
    int fadeValue = 1;

    [HideInInspector]
    public static UICharacterManager instance;

	// Use this for initialization
	void Start () 
    {
        instance = this;
        m_currentTime = 0;
	}
	
	public void UpdateStatus(float hpClampt)
    {
        currentHp = hpClampt;
        int hp = Mathf.RoundToInt(hpClampt*100f);
        m_hpText.text = hp.ToString();

        healthBar.localScale = new Vector3(hpClampt, 0.5f, 0.5f);
        
    }

    void OnDestroy()
    {
        instance = null;
    }
    void Update()
    {
        m_currentTime += Time.deltaTime * fadeValue;

        float currentBlinkLong = Mathf.Lerp(m_MinBlinkTime, m_MaxBlinkTime, currentHp);
        if (m_currentTime > currentBlinkLong || m_currentTime < 0)
        {
            if (m_currentTime > currentBlinkLong)
                m_currentTime = currentBlinkLong;
            else if (m_currentTime < 0)
                m_currentTime = 0;
            fadeValue = -fadeValue;
        }
        UpdateHeartColor(m_currentTime / currentBlinkLong);
        
    }

    void UpdateHeartColor(float value)
    {
        float lerptColor = Mathf.Lerp(1,m_MinColorValue,value);
        m_Heart.color = new Color(lerptColor, lerptColor, lerptColor);
    }
}
