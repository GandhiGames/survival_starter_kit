using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class FlameBurst : MonoBehaviour
	{
		public int DPS { get; set; }
		public float DamageTime { get; set; }

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Enemy")) {
				var otherController = other.GetComponent<DamageAnimationController> ();
				
				otherController.ApplySpecialDamage (SPECIAL_DAMAGE_TYPE.FIRE, DPS, DamageTime);
			}
		}
	}
}
