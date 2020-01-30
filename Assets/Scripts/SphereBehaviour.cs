using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour {

	TGCConnectionController controller;
	Rigidbody rb;

	private int attention1;
	private int signal1;


	void Jump ()
	{
		

		if (Input.GetKeyDown (KeyCode.Space)) {
			rb.AddForce(Vector3.up * 10,ForceMode.VelocityChange);
		}

		if (Input.GetKey (KeyCode.Space)) {
			//if (jumpValue < maxJumpValue) {
				rb.AddForce(Vector3.up * 10,ForceMode.Acceleration);
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

	void Update () {



		//Debug.Log ("teste");
		if(attention1 > 50 && this.transform.position.y < 9)
			rb.AddForce(Vector3.up * 9,ForceMode.Acceleration);
		
		Jump ();
	}
}
