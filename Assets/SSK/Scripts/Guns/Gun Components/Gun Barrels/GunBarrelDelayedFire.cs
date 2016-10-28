using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class GunBarrelDelayedFire : GunBarrel
	{

		public float MinFireDelay = 0.05f;
		public float MaxFireDelay = 0.15f;
	
		public override void OnFire ()
		{
			Invoke ("FireBarrel", Random.Range (MinFireDelay, MaxFireDelay));
		}

		private void FireBarrel ()
		{
			base.OnFire ();
		}
	}
}
