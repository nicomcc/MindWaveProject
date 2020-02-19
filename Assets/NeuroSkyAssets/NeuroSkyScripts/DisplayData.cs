using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AttentionSignalHeader;
//using CsvInsertion;
using System;
//using System.IO;
//using System.Linq;
//using System.Text;

public class DisplayData : MonoBehaviour
{
	public Texture2D[] signalIcons;


	public List<AttentionSignal> signalRecord = new List<AttentionSignal> ();
	
	private int indexSignalIcons = 1;
	
    TGCConnectionController controller;

	window_graph graph;

    private int poorSignal1;
    public int attention1;
	private int previousAttention = 0;
    private int meditation1;

	[HideInInspector] public float previousTime = 0;

	[HideInInspector] public float currentTime = 0;
    public float dataAcquisitionTime = 1f;

	private float delta;

	[HideInInspector] public bool startReading = false;
	[HideInInspector] public int startTime;
	//private bool connectStart = false;

	public int count = 0;

	public float slider;

	public Material[] skyBox;//SkyBox Materials..

    void Start()
	{
		Screen.SetResolution(1024, 768, false);
		SetRandomSkybox ();
			
		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();
		
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
    }

	void SetRandomSkybox()
	{
		//int index = Random.Range (0, skyBox.Length);
		//RenderSettings.skybox=skyBox[index];
	}
	
	void OnUpdatePoorSignal(int value){
		poorSignal1 = value;
		if(value < 25){
      		indexSignalIcons = 0;
		}else if(value >= 25 && value < 51){
      		indexSignalIcons = 4;
		}else if(value >= 51 && value < 78){
      		indexSignalIcons = 3;
		}else if(value >= 78 && value < 107){
      		indexSignalIcons = 2;
		}else if(value >= 107){
      		indexSignalIcons = 1;
		}
	}

	void OnUpdateAttention(int value){
		attention1 = value;
	}
	void OnUpdateMeditation(int value){
		meditation1 = value;
	}
	void OnUpdateDelta(float value){
		delta = value;
	}

	public void slider_change(float value)
	{
		slider = value;
	}


    void OnGUI()
    {

		//attention1 = (int)slider;

		GUILayout.BeginHorizontal();
		
		
        if (GUILayout.Button("Connect"))
        {
            controller.Connect();
			//connectStart = true;

        }
        if (GUILayout.Button("Disconnect"))
        {
            controller.Disconnect();
			indexSignalIcons = 1;
			//connectStart = false;
        }
			

		/*if (GUILayout.Button("Data Acquisition"))
		{			
			startReading = !startReading;
			startTime = (int)Time.time;
		}*/

		//if (GUILayout.Button("Clear Data"))
		//{	
			/*GameObject[] dots = GameObject.FindGameObjectsWithTag ("dot");
			GameObject[] connections = GameObject.FindGameObjectsWithTag ("connection");

			for (int i = 0; i < dots.Length; i++) 
				Destroy (dots[i]);
			for (int i = 0; i < connections.Length; i++) 
				Destroy (connections[i]);*/

			//List<GameObject> cList = graph.circleList;
			//Debug.Log (cList.Count);

			/*for (int i = 0; i < graph.circleList.Count; i++)
				;
				//Destroy (graph.circleList [i]);
			for (int i = 0; i < graph.circleList.Count; i++);
				//graph.circleList.RemoveAt(i);

			for (int i = 0; i < graph.connectionList.Count; i++)
				;
				//Destroy (graph.connectionList[i]);
			for (int i = 0; i < graph.connectionList.Count; i++);
				//graph.connectionList.RemoveAt(i);?*/

		//	signalRecord.Clear ();
		//}

		

		/*if (GUILayout.Button("Change Background"))
		{			
			SetRandomSkybox ();
		}*/


		
		GUILayout.Space(Screen.width-250);
		GUILayout.Label(signalIcons[indexSignalIcons]);
		
		GUILayout.EndHorizontal();

		
        GUILayout.Label("PoorSignal1:" + poorSignal1);
        GUILayout.Label("Attention1:" + attention1);
       // GUILayout.Label("Meditation1:" + meditation1);
		//GUILayout.Label("Delta:" + delta);		


		//if(poorSignal1 <= 25 && startReading)
		if(startReading)
		{

			//currentTime = Time.time - startTime;
			currentTime = Time.time;

			if (currentTime - previousTime > dataAcquisitionTime) 
			{
				previousTime = currentTime;
				AttentionSignal sig = new AttentionSignal (attention1, (int)(currentTime - startTime), poorSignal1);
				signalRecord.Add (sig);
				previousAttention = attention1;
				count++;
			}
				
		}
    }


}
