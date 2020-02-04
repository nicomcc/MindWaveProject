using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleMovement : MonoBehaviour {

	// Use this for initialization
	public Transform Target;
	public Vector3 offset;
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Target.position + offset;
	}
}
