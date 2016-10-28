using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SurvivalKit
{
	public class GameOverHandler : MonoBehaviour
	{
		public GameObject gameOverUI;
		public GameObject[] otherText;
		public Text gameoverText;
		public UIFlash flash;

		//public float timeBetweenGameoverAndSceneReset = 3f;

		private static readonly string PRE_ON_DEAD_TEXT = "You Survived ";
		private static readonly string POST_ON_DEAD_TEXT_PLURAL = " Rounds!";
		private static readonly string POST_ON_DEAD_TEXT_SINGULAR = " Round!";
		private static readonly string GAME_OVER_TEXT = "Game Over";

		void Awake ()
		{
			gameoverText.enabled = false;
			gameOverUI.SetActive (false);
		}

		void Update ()
		{
			if (Input.GetKeyUp (KeyCode.R)) {
				OnGameOver ();
			}
		}
		
		public void OnGameOver ()
		{
			flash.GameOverUIFlash ();

			foreach (var t in otherText) {
				t.SetActive (false);
			}

			gameOverUI.SetActive (true);

//			var roundsSurvived = RoundDirector.instance.currentRoundNumber;

			if (Highscore.instance == null) {
				gameObject.AddComponent<Highscore> ();
			}


			Highscore.instance.Save (0);

			var postText = (0 == 1) ? POST_ON_DEAD_TEXT_SINGULAR : POST_ON_DEAD_TEXT_PLURAL;

			ShowText (GAME_OVER_TEXT);

			StartCoroutine (ShowTextInSeconds (1.2f, PRE_ON_DEAD_TEXT + 0 + postText));
		}

		private IEnumerator ShowTextInSeconds (float seconds, string text)
		{
			yield return new WaitForSeconds (seconds);
			ShowText (text);
		}

		private void ShowText (string text)
		{
			gameoverText.enabled = true;
			gameoverText.text = text;
		}


		public void Restart ()
		{
			Application.LoadLevel ("Game Scene");
		}

		public void LoadMainMenu ()
		{
			Application.LoadLevel ("Main Menu");
		}


	}
}
