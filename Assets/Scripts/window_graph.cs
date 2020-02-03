using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using AttentionSignalHeader;

public class window_graph : MonoBehaviour {

	public GameObject signalReadObject;
	public DisplayData data;

	private RectTransform graphContainer;

	private List<AttentionSignal> list;

	[SerializeField]private Sprite circleSprite;

	private float previousTime = 0;

	private int previousCount = 0;

	private List<GameObject> circleList = new List<GameObject>();
	private List<GameObject> connectionList = new List<GameObject>();

	private void Awake()
	{
		graphContainer = transform.Find ("graphContainer").GetComponent<RectTransform> ();

		List<int> valueList = new List<int> () { 10, 30, 50, 30, 60, 70, 40, 100, 33, 55, 44, 22, 33, 44, 22, 88, 32, 44, 76};


		data = signalReadObject.GetComponent<DisplayData>();
		list = data.signalRecord;
		//ShowGraph (list);
		previousTime = data.currentTime;
	}


	private GameObject CreateCircle(Vector2 anchoredPosition)
	{
		GameObject gameObject = new GameObject ("Circle", typeof(Image));
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

		GameObject lastCircleGameObject = null;
		for (int i = 0; i < valueList.Count; i++)
		{
			float xPosition = (i / xSize) * graphWidth;
			float yPosition = (list[i].attention / yMaximum) * graphHeight;
			GameObject circleGameObject = CreateCircle (new Vector2 (xPosition, yPosition));
			if (lastCircleGameObject != null)
			{
				CreateDotConnection (lastCircleGameObject.GetComponent<RectTransform> ().anchoredPosition, circleGameObject.GetComponent<RectTransform> ().anchoredPosition);
			}
			circleList.Add (circleGameObject);
			lastCircleGameObject = circleGameObject;
		}
	}

	private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
	{
		GameObject gameObject = new GameObject ("dotConnection", typeof(Image));
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
		connectionList.Add (gameObject);
	}



	void Update () {

		if (data.count != previousCount) 
		{
			previousCount = data.count;


			for (int i = 0; i < circleList.Count; i++) 
				Destroy (circleList[i]);
			for (int i = 0; i < circleList.Count; i++)
				circleList.RemoveAt(i);

			for (int i = 0; i < connectionList.Count; i++) 
				Destroy (connectionList[i]);
			for (int i = 0; i < connectionList.Count; i++)
				connectionList.RemoveAt(i);

			list = data.signalRecord;
			ShowGraph (list);
		}
			
	}

}
