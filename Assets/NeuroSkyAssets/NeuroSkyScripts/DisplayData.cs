using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AttentionSignalHeader;

public class DisplayData : MonoBehaviour
{
	public Texture2D[] signalIcons;


	public List<AttentionSignal> signalRecord = new List<AttentionSignal> ();
	
	private int indexSignalIcons = 1;
	
    TGCConnectionController controller;

    private int poorSignal1;
    public int attention1;
	private int previousAttention = 0;
    private int meditation1;

	[HideInInspector] public float previousTime = 0;

	[HideInInspector] public float currentTime = 0;
    public float dataAcquisitionTime = 1f;

	private float delta;

	public bool startReading = false;
	private int startTime;
	//private bool connectStart = false;

	public int count = 0;

	public float slider;

	public Material[] skyBox;//SkyBox Materials..

    void Start()
	{
		int index = Random.Range (0, skyBox.Length);
		RenderSettings.skybox=skyBox[index];
		Debug.Log(index);
			
		controller = GameObject.Find("NeuroSkyTGCController").GetComponent<TGCConnectionController>();
		
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
		
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
        if (GUILayout.Button("DisConnect"))
        {
            controller.Disconnect();
			indexSignalIcons = 1;
			//connectStart = false;
        }
		if (GUILayout.Button("Data Acquisition"))
		{			
			startReading = !startReading;
		//	startTime = (int)Time.time;
		}

		if (GUILayout.Button("Clear Data"))
		{			
			signalRecord.Clear ();

		}


		
		GUILayout.Space(Screen.width-250);
		GUILayout.Label(signalIcons[indexSignalIcons]);
		
		GUILayout.EndHorizontal();

		
        GUILayout.Label("PoorSignal1:" + poorSignal1);
        GUILayout.Label("Attention1:" + attention1);
        GUILayout.Label("Meditation1:" + meditation1);
		//GUILayout.Label("Delta:" + delta);
			


		if(poorSignal1 <= 25 && startReading)
		{

			//currentTime = Time.time - startTime;
			currentTime = Time.time;

			if (currentTime - previousTime > dataAcquisitionTime) 
			{
				previousTime = currentTime;

				AttentionSignal sig = new AttentionSignal (attention1, (int)currentTime);
				signalRecord.Add (sig);
				previousAttention = attention1;
				count++;
			}
				
		}


    }
}
