using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public abstract class DamageTurret : Turret
	{
		public int ProjectileDamage = 2;
		
		
		protected override void UpdateProjectile (GameObject bullet, bool setOwner)
		{
			var projectile = bullet.GetComponent<Projectile> ();
			
			if (setOwner)
				projectile.Owner = this;
			
			projectile.DamageAmount = ProjectileDamage;
			projectile.OwnerID = gameObject.GetInstanceID ();
		}
	}
}
