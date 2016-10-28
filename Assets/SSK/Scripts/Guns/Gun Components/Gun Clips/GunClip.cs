using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class GunClip : GunComponent
	{	
		public int BulletsInClip = 15;
	
		public bool SupportReload = false;
	
		public float ReloadSpeed = 0.2f;
	
		private bool reloading = false;

		private int currentBulletsInClip;

		private GameObject projectile;

		/// <summary>
		/// The pooled objects currently available.
		/// </summary>
		private List<GameObject> pooledBullets;
	

		private static readonly int INITIAL_POOL_MAX = 20;

		void Awake ()
		{

			foreach (Transform sibling in transform) {
				if (sibling.CompareTag ("GunProjectile")) {
					projectile = sibling.gameObject;
					break;
				}
			}
		
			var poolAmount = (BulletsInClip > INITIAL_POOL_MAX) ? INITIAL_POOL_MAX : BulletsInClip;
		
			if (!projectile) {
				Debug.LogError ("Gun clip requires child bullet object with tag 'GunProjectile'");
			} else {
	
				pooledBullets = new List<GameObject> ();
				
				for (int n = 0; n < poolAmount; n++) {
					GameObject newObj = (GameObject)Instantiate (projectile);
					SetOwner (newObj);
					PoolObject (newObj);
				}
				

			}

		}
	
		void Update ()
		{
			if (!SupportReload)
				return;
	
			if (Input.GetButtonUp ("Reload") && OkToReload ()) {
				StartCoroutine (Reload ());
			}
		}
	
		private bool OkToReload ()
		{
			return currentBulletsInClip != BulletsInClip && !reloading;
		}
	
		private IEnumerator Reload ()
		{
			reloading = true;
			Debug.Log ("Reloading");
			yield return new WaitForSeconds (ReloadSpeed);
		
			currentBulletsInClip = BulletsInClip;
			reloading = false;
		}

		private void SetOwner (GameObject bullet)
		{
			var projectile = bullet.GetComponent<GunProjectile> ();

			if (!projectile) {
				Debug.LogError ("Bullet should have Projectile script attached");
			} else {
				projectile.Owner = this;
			}
		}

		/// <summary>
		/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
		/// </summary>
		/// <param name='obj'>
		/// Object to be pooled.
		/// </param>
		public void PoolObject (GameObject obj)
		{
			obj.SetActive (false);
			obj.transform.SetParent (transform);
			pooledBullets.Add (obj);
		}

		public GameObject GetBullet ()
		{
			if (pooledBullets == null)
				return null;

			if (pooledBullets.Count > 0) {
				GameObject pooledObject = pooledBullets [0];
			
				if (pooledObject) {
					pooledBullets.RemoveAt (0);
					pooledObject.transform.SetParent (null, false);
					pooledObject.SetActive (true);
				} 
			
				return pooledObject;
			} else {
				var newObj = (GameObject)Instantiate (projectile);
				newObj.SetActive (true);
				SetOwner (newObj);
				return newObj;
			}
		

		}

		public GameObject RequestBullet ()
		{
			if (reloading)
				return null;
		
			if (currentBulletsInClip > 0) {
				currentBulletsInClip--;
				var bullet = GetBullet ();
				return bullet;
			}

			return null;
		}
	
		public override void OnPickup ()
		{
			currentBulletsInClip = BulletsInClip;
		}

		public override void OnDrop ()
		{

		}
	}
}
