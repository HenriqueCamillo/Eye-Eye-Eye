using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour
{
	public int maxHealth = 100;
	public int currentHealth;
	public HealthBar healthBar;
	public float timeInvincible = 1.0f;
    bool isInvincible;
    float invincibleTimer;
    
	//public AudioSource tickSource;
	
    void Start()
    {
		currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
		
		//tickSource = GetComponent<AudioSource> ();
		
    }
    
    void Update()
    {
		if (isInvincible) //Decrementa o valor da invencibilidade
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
		        
    }
	
	public void TakeDamage(int damage) //Verifica invencibilidade
	{
		if (damage > 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
		
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
		
		//tickSource.Play ();
		
		if (currentHealth <= 0)
		{
			Debug.Log("GAME OVER");
			SceneManager.LoadScene("GameOver");
			
			
		}
		
	}
}
