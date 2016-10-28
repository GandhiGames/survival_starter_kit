using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PlayerSeekingGunProjectile : GunProjectile
	{		
		public float Damage = 10f;
		public float Velocity = 40f;
		public GameObject DamageAnimation;
		private Transform target;

		private LineRenderer linerenderer;

		private bool foundTarget = false;

		
		public override void Awake ()
		{
			base.Awake ();

			if (!DamageAnimation) {
				Debug.Log ("No damage animation selected for projectile: no animation will be played on impact");
			}
		}

		
		void OnDisable ()
		{
			foundTarget = false;
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


		private Transform GetNearestObject (string tag)
		{
			var objs = GameObject.FindGameObjectsWithTag (tag);

			Transform closest = null;

			float closestDistance = float.MaxValue;

			foreach (var obj in objs) {

				var heading = obj.transform.position - transform.position;
				var distance = heading.magnitude;

				if (distance < closestDistance && IsTargetInFront (obj.transform)) {
					closestDistance = distance;
					closest = obj.transform;
					foundTarget = true;
				}
			}

			return closest;
		}

		public override void Update ()
		{
			base.Update ();
		
			if (foundTarget && target != null) {			
				Vector3 dir = target.position - transform.position;
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg - 90;
				transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			} else if (!foundTarget) {
				target = GetNearestObject ("Enemy");
			}

			GetComponent<Rigidbody2D> ().AddForce (transform.up * Velocity);

		} 


		public Vector2 GetForce ()
		{
			return target.position - transform.position;
		}
		
		private bool IsTargetInFront (Transform target)
		{
			var heading = target.position - transform.position;
		
			var dot = Vector2.Dot (heading, transform.up);

			return dot > 1.2f; 
		}
		
	
		
		public override void OnPickup ()
		{

		}

		public override void OnDrop ()
		{

		}

	}
}
