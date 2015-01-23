using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed = 20f;
    public bool spawned = false;

    public GameObject m_DamagePrefab;
    public AudioClip m_destoryClip;

    public float m_TimeToDestory = 10f;

    Owner owner = Owner.Enemy;
    int damage = 10;

    float time = 0;

    

    public void Spawn(Owner owner, int damage, bool facingRight)
    {
        this.owner = owner;
        this.damage = damage;

        if (!facingRight)
            rigidbody2D.velocity = new Vector2(-speed, 0);
        else
            rigidbody2D.velocity = new Vector2(speed, 0);

    }

    void Start () 
    {
        spawned = false;
        time = 0;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        BaseAvatar avatar = coll.transform.GetComponent<BaseAvatar>();
        if (avatar)
        {
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
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= m_TimeToDestory)
            ProcessDestory();
    }
}
