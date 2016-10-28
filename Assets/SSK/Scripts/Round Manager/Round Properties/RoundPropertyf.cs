using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public class RoundPropertyf : RoundProperty
	{
		public float initialCount;
		public float threshold;

		private float _current;
		public float current {
			get {
				return _current; 
			}
		}

		public float CalculateAndGetCurrent ()
		{
			CalculateCurrent ();
			
			return current;
		}
		
		protected override void Initialise ()
		{
			_current = initialCount;
		}
		
		protected override void Add ()
		{
			if (_current < threshold) {
				_current += _current * (percentageChange / 100);
			} else {
				_current = threshold;
			}
		}
		
		protected override void Subtract ()
		{
			if (_current > threshold) {
				_current -= _current * (percentageChange / 100);
			} else {
				_current = threshold;
			}
		}
	}
}
