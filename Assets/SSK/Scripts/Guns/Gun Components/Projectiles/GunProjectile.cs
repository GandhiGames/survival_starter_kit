using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Rigidbody2D))]
	public abstract class GunProjectile : GunComponent
	{
		public float MaxTimeAlive = 2f;
		public bool DestroyOnEnemyImpact = true;

		private GunClip owner;
		public GunClip Owner {
			set {
				owner = value;
			}
		}


		private float currentTimeAlive;

		public virtual void Awake ()
		{
			gameObject.SetActive (false);
		}

		void OnEnable ()
		{
			currentTimeAlive = 0f;
		}

		public virtual void Update ()
		{
			currentTimeAlive += Time.deltaTime;
			if (currentTimeAlive >= MaxTimeAlive) {
				ReturnProjectile ();
			}

		}

		public virtual void OnTriggerEnter2D (Collider2D other)
		{
			/*if (other.CompareTag ("Wall")) {
				ReturnProjectile ();
			}*/
		}

		protected void ApplyDamage (Collider2D other, float damage)
		{
			other.SendMessage ("ApplyDamage", damage);
			/*var health = other.GetComponent<EnemyHealth> ();
		
			if (!health) {
				Debug.LogError ("Enemy should have health script attached");
				return;
			}
		
			health.ApplyDamage (damage);*/
		}

		protected void InitDamageAnimation (Collider2D other, GameObject animation)
		{
			var dir = transform.up.normalized;
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		
			Instantiate (animation, transform.position, Quaternion.AngleAxis (angle, Vector3.forward));
		}

		/// <summary>
		/// Gets the status of the gun in case it has been removed from scene/disabled.
		/// </summary>
		/// <returns><c>true</c>, if owner not null and gun object active.</returns>
		private bool GunActive ()
		{
			return owner != null && owner.gameObject.activeInHierarchy;
		}

		protected void ReturnProjectile ()
		{
			if (GunActive ()) {
				owner.PoolObject (gameObject);
			} else {
				Destroy (gameObject);
			}
		}
	
		public override void OnPickup ()
		{

		}

		public override void OnDrop ()
		{

		}

	}
}

