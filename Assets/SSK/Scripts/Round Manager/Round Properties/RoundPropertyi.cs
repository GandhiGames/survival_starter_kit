using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public enum Operator
	{
		None,
		Add,
		Subtract
	}

	[System.Serializable]
	public class RoundPropertyi : RoundProperty
	{
		public int initialCount;


		public int threshold;

		protected int _current;
		public int current { get { return _current; } }

		public int CalculateAndGetCurrent ()
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
				_current += (int)(_current * (percentageChange / 100));
			} else {
				_current = threshold;
			}
		}

		protected override void Subtract ()
		{
			if (_current > threshold) {
				_current -= (int)(_current * (percentageChange / 100));
			} else {
				_current = threshold;
			}
		}
	}
}
