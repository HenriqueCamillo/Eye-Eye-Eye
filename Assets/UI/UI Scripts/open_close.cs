using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class open_close : MonoBehaviour
{
	private Animator anim;
	public bool eye_lid_closed = false;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void OpenEyes()
	{
		anim.SetBool("isClosed", false);
		eye_lid_closed = false;
	}

	public void CloseEyes()
	{
		anim.SetBool("isClosed", true);
		eye_lid_closed = true;
	}
}
