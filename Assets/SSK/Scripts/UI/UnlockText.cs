using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SurvivalKit
{
	[RequireComponent (typeof(Text))]
	public class UnlockText : MonoBehaviour
	{

		public float TimeBetweenMessages = 0.4f;

		private MessageQueue _queue = new MessageQueue ();
		private bool textBeingShown { get { return _text.enabled; } }
		private Text _text;
		private Color _initialColour;
		private bool okToShowNewMessage = true;

		private TextFade _textFade;

		void Awake ()
		{
			_text = GetComponent<Text> ();
			_text.enabled = false;
			_initialColour = _text.color;
			_textFade = GetComponent<TextFade> ();
		}

		void Update ()
		{
			if (_queue.messagesQueued && !textBeingShown && okToShowNewMessage) {
				ShowCurrentMessage ();
			} 
		}

		public void AddMessage (Message message)
		{
			_queue.Push (message);
		}

		private void ShowCurrentMessage ()
		{
			okToShowNewMessage = false;

			_text.text = _queue.current.text;
			_textFade.FadeIn (FadeInComplete);
		
		}

		private void FadeInComplete ()
		{
			StartCoroutine (HideTextInSeconds (_queue.current.displayTime));
		}
		
		private IEnumerator HideTextInSeconds (float inSeconds)
		{
			yield return new WaitForSeconds (inSeconds);

			_textFade.FadeOut (FadeOutComplete);

			//HideText ();
			RemoveCurrentMessage ();
		}

		private void FadeOutComplete ()
		{
			StartCoroutine (WaitBeforeShowingNewMessage ());

		}
		private IEnumerator WaitBeforeShowingNewMessage ()
		{
			yield return new WaitForSeconds (TimeBetweenMessages);
			
			okToShowNewMessage = true;
			;
		}

		private void RemoveCurrentMessage ()
		{
			_queue.Pop ();
		}


	}
}
