using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public GameObject panel_lock;
	int a=1;
		
	void Start () //Confere se os níveis extras estão disponíveis
	{
		a = PlayerPrefs.GetInt("a",0);
		if(a>1)
			Destroy(panel_lock);
	}
	
	public void Iniciar ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		
	}
	
	public void Sair ()
	{
		Debug.Log("O jogo deve fechar :D");
		Application.Quit();
	}
	
	public void LoadMenu()
	{
		Debug.Log("Carregando o menu");
		SceneManager.LoadScene("Menu Principal");
	}
	
	public void LoadExtra1()
	{
		Debug.Log("Carregando Extra 1");
		SceneManager.LoadScene("Level_Extra_01");
	}
	
	public void LoadExtra2()
	{
		Debug.Log("Carregando Extra 2");
		SceneManager.LoadScene("Level_Extra_02");
	}
	
	public void LoadExtra3()
	{
		Debug.Log("Carregando Extra 3");
		SceneManager.LoadScene("Level_Extra_03");
	}
	
	public void Unlock() //altera e salva o valor de "a", desbloqueando os níveis extras permanentemente 
	{
		int _a=2;
		PlayerPrefs.SetInt("a",_a);
		
		Debug.Log("Níveis extras disponíveis");
		Debug.Log("Carregando o menu");
		
		SceneManager.LoadScene("Menu Principal");
		
	}
		
}
