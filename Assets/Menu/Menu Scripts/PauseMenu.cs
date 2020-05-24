using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	
	public static bool GameIsPaused = false;

    //public GameObject pauseMenuUI;
	public GameObject pauseMenu;
	
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}	
    }
	
	public void Resume()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f; //Retorna o valor do tempo ao valor inicial
		GameIsPaused = false;
	}
	
	void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f; //Pausa o jogo
		GameIsPaused = true;
	}
/*
	void Update()
	{
		if(Input.GetKeyDown("escape"))
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	public void Unpause()
	{
		Time.timeScale = 1f;
	}
*/
	public void QuitGame()
	{
		Debug.Log("quit game");
		Application.Quit();
	}
}
