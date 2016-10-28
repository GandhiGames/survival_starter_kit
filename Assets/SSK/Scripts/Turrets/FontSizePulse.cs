using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Text))]
	public class FontSizePulse : MonoBehaviour
	{
		public int minSize;
		public int maxSize;

		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text> ();
		}


		public void Pulse ()
		{
			if (!_text)
				return;

			StartCoroutine (PulseUp ());
		}

		private IEnumerator PulseUp ()
		{
			while (true) {
				_text.fontSize += 1;

				if (_text.fontSize > maxSize) {
					_text.fontSize = maxSize;
				}

				if (_text.fontSize == maxSize) {
					break;
				}

				yield return new WaitForEndOfFrame ();
			}

			StartCoroutine (PulseDown ());
		}

		private IEnumerator PulseDown ()
		{
			while (true) {
				_text.fontSize -= 1;
				
				if (_text.fontSize < minSize) {
					_text.fontSize = minSize;
				}
				
				if (_text.fontSize == minSize) {
					break;
				}
				
				yield return new WaitForEndOfFrame ();
				
			}
		}
	}
}
