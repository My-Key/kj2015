using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Panel : MonoBehaviour {

    public Transform m_Panel;
    public Vector3 m_StartPosition;
    public Vector3 m_EndPosition;
    public float m_ShowTime = 1f;
    public float m_HideTime = 1f;

    public virtual void Show(bool fade)
    {
        m_Panel.gameObject.SetActive(true);
        m_Panel.DOLocalMove(m_EndPosition, m_ShowTime);
        if (fade)
            GameManager.instance.Fade(0.8f, 1f);
    }
    public virtual void Hide(bool fade)
    {
        m_Panel.DOLocalMove(m_StartPosition, m_HideTime);
        StartCoroutine(HideObject(fade));
    }

    IEnumerator HideObject(bool fade)
    {
        yield return new WaitForSeconds(m_HideTime);
        m_Panel.gameObject.SetActive(false);
        if (fade)
            GameManager.instance.Fade(0f, 1f);
    }
	
}
