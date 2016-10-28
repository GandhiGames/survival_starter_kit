using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class GunTriggerBurst : GunTrigger
	{
		public float TimeBetweenBursts = 0.3f;
		public int BulletsPerBurst = 3;

		private bool firingBurst;

		// Use this for initialization
		public override void Awake ()
		{
			firingBurst = false;
			base.Awake ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (!inUse)
				return;
		
			if (ShootType == SHOOT_TYPE.CLICK) {
				HandleClickInput ();
			} else {
				HandleHoldInput ();
			}
		}

		protected override bool OkToShoot ()
		{
			return base.OkToShoot () && !firingBurst;
		}

		public override void HandleClickInput ()
		{
			if (Input.GetButtonDown (buttonMapping) && OkToShoot ()) {
				StartCoroutine (BurstFire ());
			}
		}

		public override void HandleHoldInput ()
		{
			if (Input.GetButton (buttonMapping) && OkToShoot ()) {
				StartCoroutine (BurstFire ());
			}
		}

		private IEnumerator BurstFire ()
		{
			firingBurst = true;

			for (int i = 0; i < BulletsPerBurst; i++) {

				foreach (var barrel in barrels) {
					barrel.OnFire ();
				}
			
				yield return new WaitForSeconds (DelayBetweenProjectiles);
			}
		
			yield return new WaitForSeconds (TimeBetweenBursts);
		
			firingBurst = false;
		}
	}
}
