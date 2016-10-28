using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PreperationText : RoundText
	{
		private Preperation _preperation;
		private RoundText.delegateFunc _callback;

		private static readonly string PREPERATION_PRE_TEXT = "Round Starting in:";

		public override void Awake ()
		{
			base.Awake ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (_preperation != null) {
				int seconds = _preperation.timeInSeconds;

				UpdateText (string.Format ("{0} {1}", PREPERATION_PRE_TEXT, seconds));

				if (_preperation.timeInSeconds <= 0) {
					_preperation = null;

					if (_callback != null) {
						_callback ();
						_callback = null;
					}

					_textFade.FadeOut ();
				
				}
			}
		}

		public void StartCountdown (Preperation preperation, RoundText.delegateFunc callback)
		{
			_textFade.FadeIn ();

			_preperation = preperation;
			_callback = callback;
		}
	}
}
