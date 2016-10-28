using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class Message
	{
		public string text { get; private set; }
		public float displayTime { get; private set; }

		public Message (string text, float displayTime)
		{
			this.text = text;
			this.displayTime = displayTime;
		}
	}
}
