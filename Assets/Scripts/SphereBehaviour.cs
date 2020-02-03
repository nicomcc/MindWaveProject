using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour {

	TGCConnectionController controller;
	Rigidbody rb;

	private int attention1;
	private int signal1;

	public float brainItensity = 1f;
	public float gravityForce = 9.81f;

	private float slider;

	void Jump ()
	{
		

		if (Input.GetKeyDown (KeyCode.Space)) {
			rb.AddForce(Vector3.up * brainItensity,ForceMode.VelocityChange);
		}

		if (Input.GetKey (KeyCode.Space)) {
			//if (jumpValue < maxJumpValue) {
			rb.AddForce(Vector3.up * brainItensity,ForceMode.Acceleration);
			//}

		}

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

	// Update is called once per frame

	public void slider_change(float value)
	{
		//transform.position = new Vector3 (transform.position.x, value, transform.position.z);
		slider = value;
	}

	void Update () {


//		Physics.gravity = new Vector3 (0f, -gravityForce, 0f);
//
//		if(transform.position.y <= 9)
//			rb.AddForce(Vector3.up * (attention1/100f) * brainItensity, ForceMode.Acceleration);


		if (slider > 50 && transform.position.y <= 9)
			rb.AddForce(Vector3.up * 9,ForceMode.Acceleration);

		//if (this.transform.position.y >= 9)
			//rb.AddForce(Vector3.down * 3,ForceMode.Acceleration);
			
		Jump ();
	}
}
