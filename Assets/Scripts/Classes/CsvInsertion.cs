using UnityEngine;
//using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;


namespace CsvInsertion
{
	public class Program : Component
	{
		public static void addCSVLog(int time, int attention, int signal, string filepath)
		{
			try
			{
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
				{
					file.WriteLine(time.ToString() + "," + attention.ToString() + "," + signal.ToString());
				}
			}

			catch(Exception ex)
			{
				new ApplicationException ("Failed to registed csv file, error: ", ex);
			}
		
		}
	}

}