using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	public class CashTotal : MonoBehaviour
	{
		public Text CashText;
		public int startingAmount = 0;

		private const string PRE_TEXT = "x ";
		private FontSizePulse fontPulse;

		public int CurrentCash { get; private set; }

		public GameObject cashImage;

		public Vector2 screenPosition {
			get {
				return Camera.main.ScreenToWorldPoint (cashImage.transform.position);
			}
		}
		
		private static CashTotal _instance;
		public static CashTotal instance { get { return _instance; } }



		void Awake ()
		{
			_instance = this;
			fontPulse = CashText.GetComponent<FontSizePulse> ();
		}

		void OnEnable ()
		{
			Increment (startingAmount);
		}

	
		public void Increment (int amount = 1)
		{
			CurrentCash += amount;
			UpdateUI ();
		}

		public void Decrement (int amount = 1)
		{
			CurrentCash -= amount;

			if (CurrentCash < 0)
				CurrentCash = 0;

			UpdateUI ();
		}

		private void UpdateUI ()
		{
			CashText.text = PRE_TEXT + CurrentCash;
			fontPulse.Pulse ();
		}
	

	}
}
