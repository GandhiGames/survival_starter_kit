using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class MessageQueue
	{
		private List<Message> _messages = new List<Message> ();
		public bool messagesQueued { get { return _messages.Count > 0; } }

		public Message current { get { return _messages [0]; } }
	
		public void Push (Message message)
		{
			_messages.Add (message);
		}

		public void Pop ()
		{
			if (_messages.Count == 0)
				return;

			_messages.RemoveAt (0);
		}

	}
}