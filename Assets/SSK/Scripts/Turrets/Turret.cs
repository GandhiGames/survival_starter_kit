using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	[RequireComponent (typeof(Animator))]
	public abstract class Turret : MonoBehaviour
	{
		public float ShootSpeed = 2f;
		public float TurnSpeed = 1f;
		public float SightRadius = 5f;
		public float ProjectileLauchSpeed = 200f;
		public GameObject BulletPrefab;
		public bool PoolProjectiles = true;
		//public LayerMask obstableLayers;

		protected Transform projSpawnLoc;
		private CircleCollider2D _collider2D;
		protected List<Transform> targets;
		protected Transform currentTarget;
		
		private static readonly int POOL_AMOUNT = 20;
		private static readonly int TARGET_CAP = 50;
		
		/// <summary>
		/// The pooled objects currently available.
		/// </summary>
		private List<GameObject> pooledBullets;
		
		
		// Use this for initialization
		public virtual void Awake ()
		{
			var colliders = GetComponents<CircleCollider2D> ();
			
			if (colliders.Length > 1) {
				foreach (var c in colliders) {
					if (c.isTrigger) {
						_collider2D = c;
						break;
					}
				}
			} else {
				_collider2D = colliders [0];
			}
			
			_collider2D.radius = SightRadius;

			targets = new List<Transform> ();
			projSpawnLoc = transform.GetChild (0);
			
			if (PoolProjectiles)
				LoadProjectiles ();
		
		}
		
		private void LoadProjectiles ()
		{
			pooledBullets = new List<GameObject> ();
			
			for (int n = 0; n < POOL_AMOUNT; n++) {
				GameObject newObj = (GameObject)Instantiate (BulletPrefab);
				UpdateProjectile (newObj, true);
				PoolObject (newObj);
			}
		}

		public void PoolObject (GameObject obj)
		{
			obj.SetActive (false);
			obj.transform.SetParent (transform);
			pooledBullets.Add (obj);
		}
		
		protected abstract void UpdateProjectile (GameObject bullet, bool setOwner);
		

		protected virtual void GetTarget ()
		{	

			if (targets.Count > TARGET_CAP) {
				currentTarget = targets [Random.Range (0, targets.Count)];
			}

			Transform closest = null;
			float closestDistanceSqr = Mathf.Infinity;
			var currentPosition = transform.position;
			
			for (int i = 0; i < targets.Count; i++) {
				
				var t = targets [i];
			
				if (t == null) {
					DeregisterTarget (t);
					continue;
				}
				var directionToTarget = t.transform.position - currentPosition;
				var dSqrToTarget = directionToTarget.sqrMagnitude;
				if (dSqrToTarget < closestDistanceSqr) {
					closestDistanceSqr = dSqrToTarget;
					closest = t.transform;
				}
			}
			
			currentTarget = closest;
		}

		protected virtual bool TargetDestroyed ()
		{
			return currentTarget == null || !currentTarget.gameObject.activeSelf;
		}

		protected bool RotateTowardsTarget ()
		{
			var heading = currentTarget.transform.position - transform.position;
			var distance = heading.magnitude;
			var dir = heading / distance;
			
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;

			var targetRot = Quaternion.AngleAxis (angle, Vector3.forward);

			transform.rotation = Quaternion.Slerp (transform.rotation, targetRot, Time.deltaTime * TurnSpeed);

			return WithinConfines (Mathf.Abs (targetRot.z), Mathf.Abs (transform.rotation.z), 0.001f);
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Enemy")) {
				RegisterTarget (other.transform);
			}
		}
		
		void OnTriggerExit2D (Collider2D other)
		{
			if (other.CompareTag ("Enemy")) {
				DeregisterTarget (other.transform);
			}
		}
		
		public virtual void Fire ()
		{
			var bullet = GetBullet ();
			bullet.transform.position = projSpawnLoc.position;
			bullet.transform.rotation = projSpawnLoc.rotation;
			bullet.GetComponent<Rigidbody2D> ().AddForce (transform.right * ProjectileLauchSpeed);
		}
		
		protected GameObject GetBullet ()
		{
			if (pooledBullets == null || !PoolProjectiles) {
				return CreateNewBullet (false);
			}
			
			if (pooledBullets.Count > 0) {
				GameObject pooledObject = pooledBullets [0];
				
				if (pooledObject) {
					pooledBullets.RemoveAt (0);
					pooledObject.transform.SetParent (null, false);
					pooledObject.SetActive (true);
				} 
				
				return pooledObject;
			} else {
				return CreateNewBullet (true);
			}

		}
		
		private GameObject CreateNewBullet (bool setOwner)
		{
			var newObj = (GameObject)Instantiate (BulletPrefab);
			newObj.SetActive (true);
			UpdateProjectile (newObj, setOwner);
			return newObj;
		}

		private bool WithinConfines (float a, float b, float confines)
		{
			return (a > (b - confines) && (a <= (b + confines)));
		}
		
		/*protected bool ClearPathToEnemy (Vector2 direction)
		{
			if (TargetDestroyed ())
				return false;

			var ray = new Ray2D (transform.position, direction);
			
			Debug.DrawRay (ray.origin, ray.direction, Color.red);
			
			var hit = Physics2D.Raycast (ray.origin, ray.direction, distanceToTarget (), obstableLayers);
			
			if (hit.collider == null) {
				return true;
			} else {
				return false;
			}
		}*/
		
		protected bool NoTargets ()
		{
			return targets.Count == 0;
		}
		
		protected void RegisterTarget (Transform target)
		{
			targets.Add (target);
		}
		
		protected void DeregisterTarget (Transform target)
		{
			targets.Remove (target);
		}

		public virtual void OnActive ()
		{
		}

		private float distanceToTarget ()
		{
			var heading = currentTarget.transform.position - transform.position;
			return heading.magnitude;
		}
		
		
	}
}
