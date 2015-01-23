using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed = 20f;


    public GameObject m_DamagePrefab;
    public AudioClip m_destoryClip;

    public float m_TimeToDestory = 10f;

    Owner owner = Owner.Enemy;
    int damage = 10;

    float time = 0;
    bool m_facingRight = true;

    bool isKnife = false;


    public void Spawn(Owner owner, int damage, bool facingRight)
    {
        time = 0;
        this.owner = owner;
        this.damage = damage;

        if (m_facingRight != facingRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (!facingRight)
        {
            rigidbody2D.velocity = new Vector2(-speed, 0);
            m_facingRight = false;
        }
        else
            rigidbody2D.velocity = new Vector2(speed, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        BaseAvatar avatar = other.transform.GetComponent<BaseAvatar>();
        if (avatar)
        {
            if (avatar.m_Owner == owner)
                return;
            avatar.Damage(owner, damage);
        }
        ProcessDestory();
    }

    void ProcessDestory()
    {
        if (m_DamagePrefab)
            PoolManager.Instantiate(m_DamagePrefab, transform.position, Quaternion.identity);
        if (m_destoryClip)
            AudioSource.PlayClipAtPoint(m_destoryClip, transform.position);
        if (isKnife)
            collider2D.enabled = false;
        else
            PoolManager.Destroy(this.gameObject);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= m_TimeToDestory)
            ProcessDestory();
    }

    public void UseKnife(Owner owner, int damage)
    {
        isKnife = true;
        collider2D.enabled = true;
    }
}
