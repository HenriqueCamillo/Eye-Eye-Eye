using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	
	public static bool GameIsPaused = false;

    //public GameObject pauseMenuUI;
	public GameObject pauseMenu;
	[SerializeField] Canvas canvas;
	
	void Start()
	{
		canvas = GetComponent<Canvas>();
	}

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
		canvas.sortingOrder = 9;
	}
	
	void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0f; //Pausa o jogo
		GameIsPaused = true;
		canvas.sortingOrder = 11;
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

	public void LoadMenu()
	{
		SceneManager.LoadScene(0);
		SoundManager.instance.PlayMenuMusic();
	}
}
