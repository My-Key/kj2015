using UnityEngine;
using System.Collections;

public class PlayerControl : BaseAvatar
{

	void Awake()
	{
		groundCheck = transform.Find("groundCheck");
		m_anim = GetComponent<Animator>();
        m_Owner = Owner.Player;
	}


	public override void Update()
	{
        base.Update();

        if (Input.GetKeyDown(KeyCode.W) && grounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.Space))
            FireWeapon();
	}


    public override void FixedUpdate()
	{
        if (m_MoveValue * rigidbody2D.velocity.x < maxSpeed)
            rigidbody2D.AddForce(Vector2.right * m_MoveValue * moveForce);
        m_MoveValue = Input.GetAxis("Horizontal");
		m_anim.SetFloat("Speed", Mathf.Abs(m_MoveValue));

        base.FixedUpdate();

	}
	

    void Jump ()
    {
        m_anim.SetTrigger("Jump");
        //int i = Random.Range(0, jumpClips.Length);
        //AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

        rigidbody2D.AddForce(new Vector2(0f, jumpForce));
    }


    
}
