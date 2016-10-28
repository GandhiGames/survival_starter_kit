using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public enum Gun_Type
	{
		ONE_HANDED,
		TWO_HANDED,
		DUAL_WIELD
	}

/// <summary>
/// Calls all child GunComponents OnPickup and OnDrop methods when gun is picked up and dropped respectively.
/// Stores the guns type i.e. one handed, two handed, dual wield. This is used by the Holster script to update
/// the players sprite.
/// </summary>
	[RequireComponent (typeof(Collider2D))]
	public class Gun : MonoBehaviour
	{

		public Gun_Type GunType;

		private GunComponent[] gunComponents;
		
		private Collider2D _collider;
		
		private static readonly float ON_DROP_PICKUP_DELAY = 1.5f;

		void Awake ()
		{
			gunComponents = GetComponentsInChildren<GunComponent> ();
			_collider = GetComponent<Collider2D> ();
		}

		public void OnPickup ()
		{
			foreach (var component in gunComponents) {
				component.OnPickup ();
			}
		}

		public void OnDrop ()
		{
			_collider.enabled = false;
		
			Invoke ("EnableCollider", ON_DROP_PICKUP_DELAY);
	
			foreach (var component in gunComponents) {
				component.OnDrop ();
			}
		}
	
		private void EnableCollider ()
		{
			_collider.enabled = true;
		}
	}
}
