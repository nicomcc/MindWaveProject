using UnityEngine;
using System.Collections;
using System;


namespace AttentionSignalHeader

{
	
	public class AttentionSignal : Component
	{
		public int attention;
		public float time;
		public int signal;

		public AttentionSignal()
		{
			attention = 0;
			time = 0;
			signal = 0;
		}	

		public AttentionSignal(int att, int tim, int sig)
		{
			attention = att;
			time = tim;
			signal = sig;
		}	


		public void SetAttentionData(int att, int tim, int sig)
		{
			attention = att;
			time = tim;
			signal = sig;
		}

	}
}