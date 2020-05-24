using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pepper : MonoBehaviour 
{
	//bool isInvincible;
	
	//public AudioSource tickSource;
	
	/*
	void Start (){
		tickSource = GetComponent<AudioSource> ();
	}
	*/
	
    void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player")){
			other.gameObject.GetComponent<Player_Health>().TakeDamage(20);
			//tickSource.Play ();
		}
		
	}
}
