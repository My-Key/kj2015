using UnityEngine;
using System.Collections;

public class BaseAvatar : MonoBehaviour {

    public int m_HP = 100;
    public Owner m_Owner = Owner.Enemy;

    public bool facingRight = true;
    public bool jump = false;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public int m_meeleDamage = 10;
    
    public GameObject rocket;
    public float rocketSpeed = 20f;
    public int m_weaponDamage = 50;


    [HideInInspector]
    public Transform groundCheck;
    public bool grounded = false;
    public Animator m_anim;
    public float m_MoveValue = 0;


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
        GameObject bullet = PoolManager.Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        bullet.GetComponent<Projectile>().Spawn(m_Owner, m_weaponDamage, facingRight);
    }



    public virtual void Damage(Owner damageOwner, int damage)
    {
        if (damageOwner == m_Owner)
            return;

        m_HP -= damage;
        ProcessDamage();
    }



    public virtual void ProcessDamage()
    {
        if (m_HP <= 0)
        {
            DeadAnim();
        }
        else
            DamageAnim();
    }



    public virtual void DeadAnim()
    {
        AnimationClip ac = m_anim.animation.GetClip("Dead");
        if (ac)
            m_anim.Play("Dead");
    }



    public virtual void DamageAnim()
    {
        AnimationClip ac = m_anim.animation.GetClip("Damage");
        if (ac)
            m_anim.Play("Damage");
    }

}



public enum Owner
{
    Player,
    Enemy,
    NPC
}
