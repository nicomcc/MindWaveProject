using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour {

	TGCConnectionController controller;
	Rigidbody rb;

	private int attention1;
	private int signal1;

	public int repelValue = 50;

	public GameObject particle;

	private float slider;

	void Jump ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
			rb.AddForce(new Vector3(-Physics.gravity.x/1f, 0f, -Physics.gravity.z/1f) ,ForceMode.VelocityChange);
		
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();

		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
	}

	void OnUpdateAttention(int value){
		attention1 = value;
	}

	void OnUpdatePoorSignal(int value){
		signal1 = value;
	}
		

	public void slider_change(float value)
	{
		slider = value;
	}


	void Update () {

		//attention1 = (int)slider;

		if (transform.position.x <= 28) 	
			rb.useGravity = false;		
		 else 
			rb.useGravity = true;		

		if (transform.position.x <= 28) 
			particle.SetActive (true);		
		else
			particle.SetActive (false);
		
		//rb.useGravity = (transform.position.x <= -11) ?  ((false; particle.SetActive (true)) :  true);

		if (attention1 > repelValue && transform.position.x <= 40f) 
			rb.AddForce (new Vector3 (-Physics.gravity.x / 30f, 0f, -Physics.gravity.z / 30f), ForceMode.VelocityChange);

		
			
		Jump ();
	}
}
