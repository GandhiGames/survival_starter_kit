using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class GunTriggerSingle : GunTrigger
	{

		private float currentShootSpeed;

		public override void Awake ()
		{
			base.Awake ();
		}
	
		void Update ()
		{
			if (!inUse)
				return;
			
			currentShootSpeed += Time.deltaTime;

			if (ShootType == SHOOT_TYPE.CLICK) {
				HandleClickInput ();
			} else {
				HandleHoldInput ();
			}
		}

		protected override bool OkToShoot ()
		{
			if (currentShootSpeed >= DelayBetweenProjectiles && base.OkToShoot ()) {
				currentShootSpeed = 0f;
				return true;
			}

			return false;
		}

		public override void HandleClickInput ()
		{
			if (Input.GetButtonDown (buttonMapping) && OkToShoot ()) {
				foreach (var barrel in barrels) {
					barrel.OnFire ();
				}
			}
		}

		public override void HandleHoldInput ()
		{
			if (Input.GetButton (buttonMapping) && OkToShoot ()) {
				foreach (var barrel in barrels) {
					barrel.OnFire ();
				}
			}
		}

		public override void OnPickup ()
		{
			currentShootSpeed = DelayBetweenProjectiles;
			base.OnPickup ();
		}


	}
}
