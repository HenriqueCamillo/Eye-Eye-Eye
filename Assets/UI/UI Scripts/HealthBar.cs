using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Gradient gradient;
	public bool eye_lid_closed;

	public Image fill;
	public Image openEye;
	public Image closedEye;
	public Image powerUp;

	private Color tempOEcolor;
	private Color tempCEcolor;
	private Color tempPUcolor;

	public open_close EyeController;

	void Start()
	{
		tempOEcolor = new Vector4(255, 255, 255, 1);
		tempCEcolor = new Vector4(255, 255, 255, 0);
		openEye.color = tempOEcolor;
		closedEye.color = tempCEcolor;
		SetPowerUpVisibility(false);
	}

	void Update()
	{
		if(EyeController.eye_lid_closed == true)
		{
			tempOEcolor = new Vector4(255, 255, 255, 0);
			tempCEcolor = new Vector4(255, 255, 255, 1);
			openEye.color = tempOEcolor;
			closedEye.color = tempCEcolor;
		}
		else if(EyeController.eye_lid_closed == false)
		{
			tempOEcolor = new Vector4(255, 255, 255, 1);
			tempCEcolor = new Vector4(255, 255, 255, 0);
			openEye.color = tempOEcolor;
			closedEye.color = tempCEcolor;
		}

		if(Input.GetKeyDown("mouse 0"))
		{
			SetPowerUpVisibility(true);
		}
		if(Input.GetKeyDown("mouse 1"))
		{
			SetPowerUpVisibility(false);
		}
	}

	public void SetMaxHealth(int health)
	{
		slider.maxValue = health;
		slider.value = health;

		fill.color = gradient.Evaluate(1f);
	}

	public void SetHealth(int health)
	{
		slider.value = health;

		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

	public void SetPowerUpVisibility(bool isVisible)
	{
		if(isVisible == true)
		{
			tempPUcolor = new Vector4(255, 255, 255, 1);
			powerUp.color = tempPUcolor;
		}
		else if(isVisible == false)
		{
			tempPUcolor = new Vector4(255, 255, 255, 0);
			powerUp.color = tempPUcolor;
		}
	}
}
