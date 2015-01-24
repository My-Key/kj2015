using UnityEngine;
using System.Collections;

public class PlayerReseter : MonoBehaviour 
{

    void OnTriggerEnter2D(Collider2D other)
    {
        BaseAvatar avatar = other.transform.GetComponent<BaseAvatar>();
        if (avatar)
        {
            if (avatar.m_Owner == Owner.Player)
            {
                GameLoader.instance.ResetPlayer();
            }
            else
                PoolManager.Destroy(avatar.gameObject);
            
        }
    }
}
