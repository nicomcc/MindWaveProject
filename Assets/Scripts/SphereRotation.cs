using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotation : MonoBehaviour {
	
	public float rotationSpeed = 25f;
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0f, rotationSpeed, 0f) * Time.deltaTime);
	}
}
