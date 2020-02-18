using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SoundHandler : MonoBehaviour {



	//private Microphone mic;

	private AudioSource audioSource;

	DisplayData controlData;



	public float soundSlider;

	public float audioTime;
	public float audioSection;

	private float startRecordTime = 0, startPlayTime = 0;

	private window_graph graphData;
	public GameObject graphObject;

	private bool isRecording = false;

	private float recordTime = 0;

	void Awake()	
	{		
		controlData = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<DisplayData> ();
		graphData = graphObject.GetComponent<window_graph> ();
	}

	public void soundSlider_change(float value)
	{
		soundSlider = value;
		audioSection = audioTime * soundSlider * 0.01f;
	}

	void Update()
	{
		if (isRecording) 
			recordTime += Time.deltaTime;
	}


	void OnGUI(){

		GUILayout.Label("");
		GUILayout.Label("");
		GUILayout.Label("");


		if (GUILayout.Button ("Record")) {
			isRecording = true;
			startRecordTime = Time.time;
			controlData.startReading = true;
			controlData.startTime = (int)Time.time;
			audioSource = GetComponent<AudioSource> ();
			audioSource.clip = Microphone.Start (Microphone.devices[0], false, 120, 44100);
		}

		if (GUILayout.Button ("Stop Recording")) {
			isRecording = false;
			controlData.startReading = false;
			Microphone.End (Microphone.devices[0]);
			audioTime = Time.time - startRecordTime;
			recordTime = 0f;
		}

		if (GUILayout.Button ("Save File")) {
			SavWav.Save ("audioLog", audioSource.clip);
		}


		if (GUILayout.Button ("Play")) {
			
			audioSource.time = audioSection;
			audioSource.Play ();
			graphData.soundMovement = true;
			graphData.startPlay = true;
		}

		if (GUILayout.Button ("Pause")) 
		{
			audioSource.Stop ();
			graphData.soundMovement = false; //soundMovement = false;
			graphData.startPlayTime = 0;
		}

		if (!isRecording)
			GUILayout.Label ("Audio Time: " + audioTime.ToString ("F1"));
		else 			
			GUILayout.Label ("Audio Time: " + recordTime.ToString ("F1"));
		
		
		GUILayout.Label ("Audio Slider Time: " + audioSection.ToString("F1"));
		GUILayout.Label ("PlayTime: " + graphData.startPlayTime.ToString("F1"));
	
	}



}
