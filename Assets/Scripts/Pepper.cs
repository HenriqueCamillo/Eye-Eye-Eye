using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pepper : MonoBehaviour 
{
	//bool isInvincible;
		
    void OnTriggerStay2D(Collider2D other)
	{
		other.gameObject.GetComponent<Player_Health>().TakeDamage(20);
		
	}
}
