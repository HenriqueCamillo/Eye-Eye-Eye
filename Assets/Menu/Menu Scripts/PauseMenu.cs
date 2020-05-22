using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;

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
}
