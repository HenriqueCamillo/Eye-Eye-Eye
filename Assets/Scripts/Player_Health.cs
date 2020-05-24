using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
	public int maxHealth = 100;
	public int currentHealth;
	public HealthBar healthBar;
	//public PlayerMovement movement;
	public float timeInvincible = 1.0f;
    bool isInvincible;
    float invincibleTimer;
    
    void Start()
    {
		currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
		
		if (currentHealth <= 0)
		{
			//movement.enabled = false;
			FindObjectOfType<GameManager>().EndGame();

            SoundManager.instance.PlayGameOverMusic();
		}
		
	}
}
