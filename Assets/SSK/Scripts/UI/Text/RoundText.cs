using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Text))]
	public class RoundText : MonoBehaviour
	{
		private Text _text;
		public delegate void delegateFunc ();
		private delegateFunc _callbackMethod;

		protected TextFade _textFade;

		public virtual void Awake ()
		{
			_text = GetComponent<Text> ();
			_textFade = GetComponent<TextFade> ();
		}

	
		public void ShowForSeconds (float seconds, string text, delegateFunc callback = null)
		{
			_callbackMethod = callback;
			StartCoroutine (_ShowForSeconds (seconds, text));
		}


		private IEnumerator _ShowForSeconds (float seconds, string text)
		{
			UpdateText (text);

			_textFade.FadeIn ();

			yield return new WaitForSeconds (seconds);

			_textFade.FadeOut ();


			if (_callbackMethod != null) {
				_callbackMethod ();
				_callbackMethod = null;
			}
		}

		public void UpdateText (string text)
		{
			_text.text = text;
		}


	}
}
