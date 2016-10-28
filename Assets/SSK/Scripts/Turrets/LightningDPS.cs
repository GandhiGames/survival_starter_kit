using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class LightningDPS : DPSTurret
	{
		private float currentDelay = 0f;
		
		void Update ()
		{
			currentDelay += Time.deltaTime;
			
			if (NoTargets ()) {
				return;
			}
			
			if (TargetDestroyed () || TargetElectrified ()) {
				GetTarget ();
				return;
			}
			
			bool facingTarget = RotateTowardsTarget ();
			
			if (facingTarget && BulletReady ()) {
				Fire ();
			} 
		}
		
		private bool TargetElectrified ()
		{
			return false;
		}
		
		private bool BulletReady ()
		{
			if (currentDelay >= ShootSpeed) {
				currentDelay = 0f;
				return true;
			}
			
			return false;
		}
		
		/*protected override void GetTarget ()
		{
			foreach (var t in targets) {
				var iceController = t.GetComponent<DamageAnimationController> ();
				
				if (iceController == null || !iceController.EncasedInIce) {
					currentTarget = t;
					break;
				}
			}
		}*/
	}
}
