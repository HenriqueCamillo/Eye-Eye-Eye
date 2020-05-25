using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void QuitGame()
	{
		Debug.Log("quit game");
		Application.Quit();
	}
	
	public void LoadMenu()
	{
		Debug.Log("Carregando o menu");
		SceneManager.LoadScene("MainMenu");
		SoundManager.instance.PlayMenuMusic();
	}
}
