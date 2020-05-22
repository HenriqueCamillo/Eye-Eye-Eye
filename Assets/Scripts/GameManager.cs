using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	bool gameHasEnded = false;
	
	public float restartDelay = 1f;
	
	public GameObject completeLevelUI;
	
	public GameObject gameOverUI;
	
	/*public void CompleteLevel () //Nível concluído
	{
		Debug.Log("LEVEL WON");
		completeLevelUI.SetActive(true);
	}
	*/
	
    public void EndGame () //Reinicia o nível
	{
		if(gameHasEnded == false)
		{
		gameHasEnded = true;
		Debug.Log("GAME OVER");
		gameOverUI.SetActive(true);
		Invoke("Restart", restartDelay);
		}
	}
	
	void Restart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}