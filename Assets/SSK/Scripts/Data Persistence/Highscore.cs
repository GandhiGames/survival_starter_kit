using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SurvivalKit
{
	public class Highscore : MonoBehaviour
	{

		public int Score { get; private set; }

		private static readonly string FILE_PATH_POST = "/leveldata.dat";

		private static Highscore _instance;
		public static Highscore instance { get { return _instance; } }

		void Awake ()
		{
			if (!_instance) {
				_instance = this;
				DontDestroyOnLoad (gameObject);
			} else if (_instance != this) {
				_instance = this;
			}
			
		}

	
		public void Load ()
		{
			
			if (File.Exists (Application.persistentDataPath + FILE_PATH_POST)) {
				Debug.Log ("Loading Data");
				
				var bf = new BinaryFormatter ();
				var file = File.Open (Application.persistentDataPath + FILE_PATH_POST, FileMode.Open);
				
				ScoreData data = (ScoreData)bf.Deserialize (file);
				
				file.Close ();
				
				this.Score = data.Round;
			}
	
		}

		public void Save (int score)
		{
			if (score > this.Score) {
				Debug.Log ("Saving Data");
			
				var bf = new BinaryFormatter ();
				var file = File.Create (Application.persistentDataPath + FILE_PATH_POST);
			
				var data = new ScoreData (score);
			
				bf.Serialize (file, data);
				file.Close ();
			}
		}
	}

	[System.Serializable]
	class ScoreData
	{
		public int Round { get; private set; }
		
		public ScoreData (int round)
		{
			Round = round;
		}
	}
}
