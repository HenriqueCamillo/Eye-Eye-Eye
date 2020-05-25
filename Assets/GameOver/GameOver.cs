using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	public void RestartGame()
	{
		SceneManager.LoadScene(0);
		SoundManager.instance.PlayMenuMusic();
	}

	public void QuitGame()
	{
		Debug.Log("quit game");
		Application.Quit();
	}
}
