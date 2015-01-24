using UnityEngine;
using System.Collections;

public class BaseAvatar : MonoBehaviour {

    public int m_MaxHP = 100;
    public int m_HP = 100;
    public Owner m_Owner = Owner.Enemy;

    public bool facingRight = true;
    public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;

    public Transform m_BulletSpawn;
    public Projectile m_KnifeSpawn;

    public int m_meeleDamage = 10;
    
    public GameObject rocket;
    public float rocketSpeed = 20f;
    public int m_weaponDamage = 50;


    [HideInInspector]
    public Transform groundCheck;
    public bool grounded = false;
    public Animator m_anim;
    public float m_MoveValue = 0;

    public GameObject m_HealthBar;

    public virtual void Awake()
    {
        groundCheck = transform.Find("groundCheck");
        m_anim = GetComponent<Animator>();
    }

    public virtual void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    }



    public virtual void FixedUpdate()
    {
        if (m_Owner != Owner.Player)
            m_MoveValue = 0;
        


        if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
            rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

        if (m_MoveValue > 0 && !facingRight)
            Flip();
        else if (m_MoveValue < 0 && facingRight)
            Flip();
    }



    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void FireWeapon()
    {
        GameObject bullet = PoolManager.Instantiate(rocket, m_BulletSpawn.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bullet.GetComponent<Projectile>().Spawn(m_Owner, m_weaponDamage, facingRight);
    }



    public void UseKnife ()
    {
        m_KnifeSpawn.UseKnife(m_Owner, m_weaponDamage);
    }



    public virtual void Damage(Owner damageOwner, int damage)
    {
        if (damageOwner == m_Owner)
            return;

        m_HP -= damage;

        if (m_HP <= 0)
        {
            m_HP = 0;
        }

        ProcessDamage();
    }



    public virtual void ProcessDamage()
    {
        Debug.Log("ProcessDamage " + m_HP);
        UpdateHPBar();
        if (m_HP <= 0)
        {
            DeadAnim();
        }
        else
            DamageAnim();
    }

    void UpdateHPBar()
    {
        float hpClampt = 0;

        if (m_HP != 0)
            hpClampt = (float)m_HP / (float)m_MaxHP;

        if (m_Owner == Owner.Enemy )
            m_HealthBar.transform.localScale = new Vector3(hpClampt, 1f, 1f);
        else if (m_Owner == Owner.Player)
        {
            UICharacterManager.instance.UpdateStatus(hpClampt);
        }
        
    }

    

    public virtual void DeadAnim()
    {
        Debug.Log("DeadAnim");
        /*AnimationClip ac = m_anim.animation.GetClip("Dead");
        if (ac)
            m_anim.Play("Dead");*/
    }



    public virtual void DamageAnim()
    {
        Debug.Log("DamageAnim");
       /* AnimationClip ac = m_anim.animation.GetClip("Damage");
        if (ac)
            m_anim.Play("Damage");*/
    }



}



public enum Owner
{
    Player,
    Enemy,
    NPC
}
