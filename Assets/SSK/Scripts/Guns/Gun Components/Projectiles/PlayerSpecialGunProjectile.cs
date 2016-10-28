using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PlayerSpecialGunProjectile : GunProjectile
	{
		public float DamagePerSecond = 0.5f;
		public float DamageTime = 1f;
		public SPECIAL_DAMAGE_TYPE DamageType;
	
		public override void OnTriggerEnter2D (Collider2D other)
		{
			base.OnTriggerEnter2D (other);
			
			if (other.CompareTag ("Enemy")) {
				var damageController = other.GetComponent<DamageAnimationController> ();
			
				if (!damageController) {
					Debug.LogError ("Enemy should have DamageAnimationController script to apply special damage");
				} else {
					damageController.ApplySpecialDamage (DamageType, DamagePerSecond, DamageTime);
				}
			
				ReturnProjectile ();
			} else if (other.CompareTag ("Destructible")) {
				ApplyDamage (other, 20);
				ReturnProjectile ();
			}
		}
	}
}