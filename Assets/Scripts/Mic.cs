using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Mic : MonoBehaviour {



	//private Microphone mic;

	private AudioSource audioSource;

	DisplayData controlData;

	public float soundSlider;

	private float audioTime;
	private float audioSection;

	void Awake()	
	{		
		controlData = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<DisplayData> ();
	}

	public void soundSlider_change(float value)
	{
		soundSlider = value;
		audioSection = audioTime * soundSlider * 0.01f;
	}

	void OnGUI(){

		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");


		if (GUILayout.Button ("Record")) {
			controlData.startReading = true;
			controlData.startTime = (int)Time.time;
			audioSource = GetComponent<AudioSource> ();
			audioSource.clip = Microphone.Start (Microphone.devices[0], false, 120, 44100);
		}
		if (GUILayout.Button ("Stop Recording")) {
			controlData.startReading = false;
			Microphone.End (Microphone.devices[0]);
			audioTime = audioSource.clip.length;
		}

		if (GUILayout.Button ("Save File")) {
			SavWav.Save ("audioLog", audioSource.clip);
		}


		if (GUILayout.Button ("Play")) {
			audioSource.time = audioSection;
			audioSource.Play ();
		}

		GUILayout.Label ("Audio Time: " + audioTime);
		GUILayout.Label ("Audio current Time: " + audioSection.ToString("F1"));
	
	}
}
