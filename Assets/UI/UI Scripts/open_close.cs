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
	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown("space") && eye_lid_closed == false)
		{
			anim.SetBool("isClosed", true);
			eye_lid_closed = true;
		}
		else if(Input.GetKeyDown("space") && eye_lid_closed == true)
		{
			anim.SetBool("isClosed", false);
			eye_lid_closed = false;
		}
	}
}
