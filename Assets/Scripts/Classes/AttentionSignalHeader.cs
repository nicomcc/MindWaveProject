using UnityEngine;
using System.Collections;
using System;


namespace AttentionSignalHeader

{
	
	public class AttentionSignal : Component
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
}