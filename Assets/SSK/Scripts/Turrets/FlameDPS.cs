using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class FlameDPS : DPSTurret
	{		
		private GameObject flameInstance;

		// Use this for initialization
		public override void Awake ()
		{
			base.Awake ();
			
			flameInstance = (GameObject)Instantiate (BulletPrefab);
			flameInstance.transform.SetParent (transform, false);
			flameInstance.transform.position = projSpawnLoc.position;
			flameInstance.transform.rotation = projSpawnLoc.rotation;

			var flameBurst = flameInstance.GetComponent<FlameBurst> ();
			
			flameBurst.DPS = DPS;
			flameBurst.DamageTime = DPSTime;

			flameInstance.SetActive (false);
		}
	
		void Update ()
		{
			if (NoTargets ()) {
				flameInstance.SetActive (false);
				return;
			}
			
			if (TargetDestroyed ()) {
				flameInstance.SetActive (false);
				GetTarget ();
				return;
			}
			
			bool facingTarget = RotateTowardsTarget ();
			
			if (facingTarget) {
				flameInstance.SetActive (true);
			} else {
				flameInstance.SetActive (false);
			}
		}

		public override void OnActive ()
		{
			flameInstance.SetActive (true);
		}
	}
}
