using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using AttentionSignalHeader;

public class window_graph : MonoBehaviour {

	public GameObject signalReadObject;
	[HideInInspector] public DisplayData data;

	private RectTransform graphContainer;

	public GameObject labelX, labelY;

	private RectTransform labelTemplateX;
	private RectTransform labelTemplateY;

	private List<AttentionSignal> list;

	[SerializeField]private Sprite circleSprite = null;

	private float previousTime = 0;

	private int previousCount = 0;

	public List<GameObject> circleList = new List<GameObject>();
	public List<GameObject> connectionList = new List<GameObject>();

	private List<GameObject> labelXList = new List<GameObject>();

	public GameObject gridX;
	public GameObject gridXContainer;

	[HideInInspector] public float slider;
	 public float soundSlider;

	//public bool dynamicLabelX = false;

	private GameObject lastCircleGameObject = null;

	private GameObject graphMarkSlider;

	private void Awake()
	{
		graphContainer = transform.Find ("graphContainer").GetComponent<RectTransform> ();

		labelTemplateX = labelX.GetComponent<RectTransform> ();
		labelTemplateY = labelY.GetComponent<RectTransform> ();										 

		data = signalReadObject.GetComponent<DisplayData>();
		list = data.signalRecord;
		previousTime = data.currentTime;
	}

	private void InstantiateSoundSlider()
	{
		float graphHeight = graphContainer.sizeDelta.y; 
		Debug.Log (graphHeight);
	    graphMarkSlider = CreateDotConnection(new Vector2 (0,graphContainer.anchoredPosition.y), new Vector2 (0, graphHeight), false);
		graphMarkSlider.GetComponent<Image> ().color = new Color (46, 46, 80, .3f);
		graphMarkSlider.GetComponent<RectTransform> ().sizeDelta = (new Vector2 (gameObject.GetComponent<RectTransform> ().sizeDelta.x, 2f));
	}

	public void UpdateSoundSlider()
	{
		float graphWidth = graphContainer.sizeDelta.x;
		float pos = graphContainer.sizeDelta.x* soundSlider * 0.01f;
		graphMarkSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2 (pos, graphContainer.sizeDelta.y * 0.5f);
		Debug.Log (soundSlider);
	}


	public void soundSlider_change(float value)
	{
		soundSlider = value;
	}

	void xDashGrid()
	{
		float graphHeight = graphContainer.sizeDelta.y; 
		float graphWidth = graphContainer.sizeDelta.x;

		float dashCount = 15;
		float xSpace = graphWidth / dashCount;
		xSpace = (graphWidth + (xSpace - gridX.GetComponent<RectTransform> ().rect.width))/dashCount;
		Debug.Log (xSpace);

		for (int j = 0; j <= 10; j++) 
		{
			for (int i = 0; i < dashCount; i++) 
			{			
				GameObject dash = Instantiate (gridX);
				RectTransform rectDash = dash.GetComponent<RectTransform> ();
				rectDash.SetParent (gridXContainer.transform);
				rectDash.anchoredPosition = new Vector2 (gridX.GetComponent<RectTransform>().anchoredPosition.x + (i * xSpace), gridX.GetComponent<RectTransform>().anchoredPosition.y  + (graphHeight * j /10 ));
				dash.SetActive (true);

			}
		}
	}

	void CreateYLabels()
	{
		int separatorCount = 10;
		float graphHeight = graphContainer.sizeDelta.y; 

		for (int i = 0; i <= separatorCount; i++) 
		{
			RectTransform labelY = Instantiate (labelTemplateY);
			labelY.SetParent (graphContainer, false);
			labelY.gameObject.SetActive (true);
			float normalizedValue = i * 1f / separatorCount;
			labelY.anchoredPosition = new Vector2 (-10f, normalizedValue * graphHeight);
			labelY.GetComponent<Text> ().text = Mathf.RoundToInt (normalizedValue * 100).ToString ();
		}
	}

	void Start()
	{
		//xDashGrid ();
		CreateYLabels ();

		//InstantiateSoundSlider ();
	}


	private GameObject CreateCircle(Vector2 anchoredPosition)
	{
		GameObject gameObject = new GameObject ("Circle", typeof(Image));
		gameObject.tag = "dot";
		gameObject.transform.SetParent (graphContainer, false);
		gameObject.GetComponent<Image> ().sprite = circleSprite;
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2 (5, 5);
		rectTransform.anchorMin = new Vector2 (0, 0);
		rectTransform.anchorMax = new Vector2 (0, 0);
		return gameObject;
	}

	private void ShowGraph(List<AttentionSignal> valueList)
	{
		float graphHeight = graphContainer.sizeDelta.y; 
		float graphWidth = graphContainer.sizeDelta.x; 

		float yMaximum = 100f;
		float xSize = valueList.Count - 1;

		float xPosition = (xSize) * graphWidth;
		float yPosition = (valueList[valueList.Count-1].attention / yMaximum) * graphHeight;

		GameObject circleGameObject = CreateCircle (new Vector2 (xPosition, yPosition));

		if (lastCircleGameObject != null)
			CreateDotConnection (lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, true);

		lastCircleGameObject = circleGameObject;

		circleList.Add (circleGameObject);

		//Reassign circle's positions
		for (int i = 0; i < circleList.Count; i++) 
		{
			xPosition = (i / xSize) * graphWidth;
			if (circleList.Count > 1) 
			{
				circleList [i].transform.position = new Vector2 (graphContainer.transform.position.x + xPosition, circleList [i].transform.position.y);
			}
		}

		//Reassign dor connection's positions, sizes and directions
		for (int i = 0; i < connectionList.Count; i++) 
		{
			if (i > 0) 
				ReassignDotConnection (connectionList [i], circleList [i - 1], circleList [i]);
			
		}

		//reassign first connection as last
		if (connectionList.Count > 0) 
			ReassignDotConnection (connectionList [0], circleList [circleList.Count - 2], circleList [circleList.Count - 1]);

	/*	if (dynamicLabelX)
		{
			GameObject labelXGameObject = Instantiate (labelX);
			RectTransform rectLabelX = labelXGameObject.GetComponent<RectTransform> ();
			rectLabelX.SetParent (graphContainer);
			rectLabelX.gameObject.SetActive (true);
			rectLabelX.anchoredPosition = new Vector3 (xPosition, -10f);
			rectLabelX.GetComponent<Text> ().text = valueList [i].time.ToString ("0");
			labelXList.Add (labelXGameObject);
		}*/
	}



	private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, bool addToList)
	{
		GameObject gameObject = new GameObject ("dotConnection", typeof(Image));
		gameObject.tag = "connection";
		gameObject.transform.SetParent (graphContainer, false);
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, .5f);
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		Vector2 dir = (dotPositionB - dotPositionA).normalized;
		float distance = Vector2.Distance (dotPositionA, dotPositionB);
		rectTransform.anchorMax = new Vector2 (0, 0); 
		rectTransform.anchorMin = new Vector2 (0, 0); 
		rectTransform.sizeDelta = new Vector2 (distance, 1f);
		rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
		rectTransform.localEulerAngles = new Vector3 (0, 0, UtilsClass.GetAngleFromVectorFloat (dir));
		if (addToList) connectionList.Add (gameObject);
		return gameObject;
	}



	private void ReassignDotConnection(GameObject dotConnectionA, GameObject dotPositionA, GameObject dotPositionB)
	{
		RectTransform rectTransformA = dotPositionA.GetComponent<RectTransform> ();
		RectTransform rectTransformB = dotPositionB.GetComponent<RectTransform> ();				 
		RectTransform rectTransformConnection = dotConnectionA.GetComponent<RectTransform> ();

		Vector2 dir = (rectTransformB.anchoredPosition - rectTransformA.anchoredPosition).normalized;
		float distance = Vector2.Distance (rectTransformA.anchoredPosition, rectTransformB.anchoredPosition);

		rectTransformConnection.sizeDelta = new Vector2 (distance, 1f);
		rectTransformConnection.anchoredPosition = rectTransformA.anchoredPosition + dir * distance * .5f;
		rectTransformConnection.localEulerAngles = new Vector3 (0, 0, UtilsClass.GetAngleFromVectorFloat (dir));
	}


	void Update () {
		//UpdateSoundSlider ();

		if (data.count != previousCount) 
		{
			previousCount = data.count;


			/*for (int i = 0; i < circleList.Count; i++) 
				Destroy (circleList[i]);
			for (int i = 0; i < circleList.Count; i++)
				circleList.RemoveAt(i);*/

			/*for (int i = 0; i < connectionList.Count; i++) 
				Destroy (connectionList[i]);
			for (int i = 0; i < connectionList.Count; i++)
				connectionList.RemoveAt(i);
			*/
			//if (dynamicLabelX) {
				for (int i = 0; i < labelXList.Count; i++)
					Destroy (labelXList [i]);
				for (int i = 0; i < labelXList.Count; i++)
					labelXList.RemoveAt (i);
			//}

			list = data.signalRecord;
			ShowGraph (list);


		}
			
	}

	/*void OnGUI()
	{
		GUILayout.Label("");	

		if (GUILayout.Button ("Clear Data")) {	
			for (int i = 0; i < circleList.Count; i++) 
				Destroy (circleList[i]);
			for (int i = 0; i < circleList.Count; i++)
				circleList.RemoveAt(i);

			for (int i = 0; i < connectionList.Count; i++) 
			Destroy (connectionList[i]);
			for (int i = 0; i < connectionList.Count; i++)
				connectionList.RemoveAt(i);

			data.signalRecord.Clear ();
		}
	}*/

}
