using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayData : MonoBehaviour
{

	public class AttentionSignal
	{
		public int attention;
		public float time;

		public AttentionSignal()
		{
			attention = 0;
			time = 0;
		}	

		public AttentionSignal(int att, int tim)
		{
			attention = att;
			time = tim;
		}	


		public void SetAttentionData(int att, int tim)
		{
			attention = att;
			time = tim;
		}

	}

	public Texture2D[] signalIcons;

	public List<AttentionSignal> signalRecord = new List<AttentionSignal> ();
	
	private int indexSignalIcons = 1;
	
    TGCConnectionController controller;

    private int poorSignal1;
    private int attention1;
	private int previousAttention = 0;
    private int meditation1;
	private float previousTime = 0;

	private float delta;

    void Start()
    {
		
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


    void OnGUI()
    {
		GUILayout.BeginHorizontal();
		
		
        if (GUILayout.Button("Connect"))
        {
            controller.Connect();
        }
        if (GUILayout.Button("DisConnect"))
        {
            controller.Disconnect();
			indexSignalIcons = 1;
        }
		
		GUILayout.Space(Screen.width-250);
		GUILayout.Label(signalIcons[indexSignalIcons]);
		
		GUILayout.EndHorizontal();

		
        GUILayout.Label("PoorSignal1:" + poorSignal1);
        GUILayout.Label("Attention1:" + attention1);
        GUILayout.Label("Meditation1:" + meditation1);
		GUILayout.Label("Delta:" + delta);


		//if (attention1 != previousAttention)
		if(Time.time - previousTime > 1)
		{
			previousTime = Time.time;

			AttentionSignal sig = new AttentionSignal (attention1, (int)Time.time);
			signalRecord.Add (sig);
			previousAttention = attention1;

		


			if (Time.time > 5) 
			{
				foreach (AttentionSignal aT in signalRecord)
				{
					//Debug.Log (aT.attention);
					Debug.Log (aT.time);
				}
			}

		}


    }
}
