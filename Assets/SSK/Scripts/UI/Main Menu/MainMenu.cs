using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	public class MainMenu : MonoBehaviour
	{
		public Text highscoreText;

		private static readonly string HIGH_SCORE_PRE_TEXT = "Highest Round: ";

		void Start ()
		{
			Highscore.instance.Load ();

			highscoreText.text = HIGH_SCORE_PRE_TEXT + Highscore.instance.Score;

			//	FloorManager.instance.GenerateFloor ();
		}
	

		public void LoadGame ()
		{
			Application.LoadLevel ("Game Scene");
		}

		public void Quit ()
		{
			Application.Quit ();
		}
	}
}
