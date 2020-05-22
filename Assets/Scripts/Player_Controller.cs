using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb;
	public float speed;
	private float moveInput;
	
	public GameObject player;
	//PlayerCtrl playerCtrl;
	
    void Start(){
		rb = GetComponent<Rigidbody2D>();

		//playerCtrl = GetComponentInParent<PlayerCtrl>();
    }
	/*
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Ground") && playerCtrl.isJumping)
		{
			playerCtrl.isJumping = false;
		}
		
		if(other.gameObject.CompareTag("Platform" && playerCtrl.isJumping))
		{
			playerCtrl.isJumping = false;
			player.transform.parent = other.gameObject.transform;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Platform"))
		{
			player.transform.parent = null;
		}
	}
	*/
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name.Equals ("Platform"))
			this.transform.parent = col.transform;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.name.Equals ("Platform"))
			this.transform.parent = null;
	}
	
	
	void FixedUpdate(){
		moveInput = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y); 
				
		
		if (rb.position.y < -2f)
		{
			FindObjectOfType<GameManager>().EndGame();
		}
	}
}

