using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	public delegate void TextFadeCallback ();

	[RequireComponent (typeof(Text))]
	public class TextFade : MonoBehaviour
	{
		public float FadeOutTime = 0.5f;
		public float FadeInTime = 0.2f;

		private Text _text;

		// Use this for initialization
		void Awake ()
		{
			_text = GetComponent<Text> ();
		}
	
		public void FadeIn (TextFadeCallback callbackOnEnd = null)
		{
			StartCoroutine (_FadeIn (callbackOnEnd));
		}

		private IEnumerator _FadeIn (TextFadeCallback callBackOnEnd)
		{
			_text.enabled = true;

			float currentTime = 0f;
			
			while (currentTime < FadeOutTime) {
				float alpha = Mathf.Lerp (0f, 1f, currentTime / FadeOutTime);
				_text.color = new Color (_text.color.r, _text.color.g, _text.color.b, alpha);
				currentTime += Time.deltaTime;
				yield return null;
			}
			
			if (callBackOnEnd != null) {
				callBackOnEnd.Invoke ();
			}
			
			
		}

		public void FadeOut (TextFadeCallback callbackOnEnd = null)
		{
			StartCoroutine (_FadeOut (callbackOnEnd));
		}

		private IEnumerator _FadeOut (TextFadeCallback callBackOnEnd)
		{
			float currentTime = 0f;
			
			while (currentTime < FadeOutTime) {
				float alpha = Mathf.Lerp (1f, 0f, currentTime / FadeOutTime);
				_text.color = new Color (_text.color.r, _text.color.g, _text.color.b, alpha);
				currentTime += Time.deltaTime;
				yield return null;
			}

			if (callBackOnEnd != null) {
				callBackOnEnd.Invoke ();
			}

			_text.enabled = false;
		
		}
	}
}
