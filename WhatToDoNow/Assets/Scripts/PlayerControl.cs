using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	
	public bool facingRight = true;			
	
	public bool jump = false;				


	public float moveForce = 365f;		
	public float maxSpeed = 5f;				
	public AudioClip[] jumpClips;			
	public float jumpForce = 1000f;			
	//public AudioClip[] taunts;				
	//public float tauntProbability = 50f;	
	//public float tauntDelay = 1f;			


	//private int tauntIndex;					
	private Transform groundCheck;			
	private bool grounded = false;			
	private Animator anim;

    public Rigidbody2D rocket;				// Prefab of the rocket.
    public float rocketSpeed = 20f;				// The speed the rocket will fire at.


	void Awake()
	{
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
            Jump();

        CheckFire();
	}


	void FixedUpdate ()
	{

        float h = Input.GetAxis("Horizontal");

		
		anim.SetFloat("Speed", Mathf.Abs(h));

		
		if(h * rigidbody2D.velocity.x < maxSpeed)
			rigidbody2D.AddForce(Vector2.right * h * moveForce);

		
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		if(h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void Jump ()
    {
        anim.SetTrigger("Jump");
        int i = Random.Range(0, jumpClips.Length);
        AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

        rigidbody2D.AddForce(new Vector2(0f, jumpForce));
    }


    void CheckFire()
    {
        // If the fire button is pressed...
        if (Input.GetButtonDown("Fire1"))
        {
            // ... set the animator Shoot trigger parameter and play the audioclip.
            //anim.SetTrigger("Shoot");
            //audio.Play();

            // If the player is facing right...
            if (facingRight)
            {
                // ... instantiate the rocket facing right and set it's velocity to the right. 
                Rigidbody2D bulletInstance = PoolManager.(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(rocketSpeed, 0);
            }
            else
            {
                // Otherwise instantiate the rocket facing left and set it's velocity to the left.
                Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bulletInstance.velocity = new Vector2(-rocketSpeed, 0);
            }
        }
    }
}
