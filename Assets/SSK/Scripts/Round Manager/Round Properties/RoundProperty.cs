using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public abstract class RoundProperty
	{
		public Operator operatorToPerform;
		public float percentageChange;
		
		private bool _initialised;
		
		
		public void CalculateCurrent ()
		{
			if (!_initialised) {
				Initialise ();
				_initialised = true;
			}
			
			Update ();
		}

		private void Update ()
		{
			switch (operatorToPerform) {
			case Operator.Add:
				Add ();
				break;
			case Operator.Subtract:
				Subtract ();
				break;
			case Operator.None:
				break;
			}
		}

		protected abstract void Initialise ();

		protected abstract void Add ();

		protected abstract void Subtract ();

	}
}