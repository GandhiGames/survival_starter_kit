using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PlayerStandardGunProjectile : GunProjectile
	{
		public float Damage;
		public GameObject DamageAnimation;

		public override void Awake ()
		{
			if (!DamageAnimation) {
				Debug.Log ("No damage animation selected for projectile: no animation will be played on impact");
			}
		
			base.Awake ();
		}

	
		public override void OnTriggerEnter2D (Collider2D other)
		{
			base.OnTriggerEnter2D (other);
		
			if (other.CompareTag ("Enemy")) {
				ApplyDamage (other, Damage);
			
				if (DamageAnimation)
					InitDamageAnimation (other, DamageAnimation);
			
				if (DestroyOnEnemyImpact)
					ReturnProjectile ();
			} else if ((other.CompareTag ("Turret") && !other.isTrigger) || other.CompareTag ("Destructible")) {
				ApplyDamage (other, Damage);

				if (DestroyOnEnemyImpact)
					ReturnProjectile ();
			}
		}
	}

	
	

	
}
