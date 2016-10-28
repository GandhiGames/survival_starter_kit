using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Text))]
	public class SimpleText : MonoBehaviour
	{
		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text> ();
		}

		public void UpdateMessage (string message)
		{
			_text.text = message;
		}

	}
}
