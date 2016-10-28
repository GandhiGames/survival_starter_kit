using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public abstract class GunTrigger : GunComponent
	{
		public enum TRIGGER_KEY_MAPPING
		{
			PRIMARY,
			SECONDARY
		}
		public TRIGGER_KEY_MAPPING KeyMapping;

		public enum SHOOT_TYPE
		{
			CLICK,
			HOLD
		}
		public SHOOT_TYPE ShootType;
	
		public float DelayBetweenProjectiles = 0.1f;

		protected string buttonMapping;
		protected bool inUse = false;
		protected List<GunBarrel> barrels;

		public virtual void Awake ()
		{
			buttonMapping = (KeyMapping == TRIGGER_KEY_MAPPING.PRIMARY) ? "ShootMain" : "ShootSecondary";

			GetBarrels ();
		}

		private void GetBarrels ()
		{

			barrels = new List<GunBarrel> ();

			foreach (Transform sibling in transform.parent) {
				if (sibling.CompareTag ("GunBarrel")) {
					var barrel = sibling.GetComponent<GunBarrel> ();

					if (!barrel) {
						Debug.LogError ("Barrel objects should have GunBarrel script attached");
					} else {
						barrels.Add (barrel);
					}
				}
			}

			if (barrels.Count == 0) {
				Debug.LogError ("Weapon requires at least one barrel with tag 'GunBarrel'");
			} 

	
		}

		/// <summary>
		/// If no menus open then ok to shoot
		/// </summary>
		/// <returns><c>true</c> can shoot <c>false</c> otherwise.</returns>
		protected virtual bool OkToShoot ()
		{
			return !TileSelector.instance.TileSelectionInProgress /*&& !GunMenuManager.instance.MenuVisible ()*/ && !PopUpMenuManager.instance.IsMenuVisible ();
		}

		public abstract void HandleClickInput ();
		public abstract void HandleHoldInput ();

		public override void OnPickup ()
		{
			inUse = true;
		}

		public override void OnDrop ()
		{
			inUse = false;
		}


	}
}
