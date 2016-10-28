using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class WeaponCrate : MonoBehaviour
	{

		public GameObject prefabToSpawn;
		public GameObject OnDestroyAnimationPrefab;
		public int numberToSpawn = 1;
		private SelectableTile owner;
		public SelectableTile Owner {
			set {
				owner = value;
				owner.Selectable = false;
			}
		}
		public float FallDamage;
		public int Health = 2;
		private float currentHealth = 0;

		void OnEnable ()
		{
			currentHealth = Health;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Player") || other.CompareTag ("Enemy")) {
				var heading = other.transform.position - transform.position;
				var distance = heading.magnitude;
				var dir = heading / distance;

				var rigidbody = other.GetComponent<Rigidbody2D> ();
				rigidbody.AddForce (dir * 400f);
				other.gameObject.SendMessage ("ApplyDamage", FallDamage);
			}
		}

		public void ApplyDamage (int amount)
		{
			currentHealth -= amount;

			if (currentHealth < 0) {
				owner.Selectable = true;
				SpawnObject ();
				Instantiate (OnDestroyAnimationPrefab, transform.position, Quaternion.identity);
				ObjectManager.instance.RemoveObject (gameObject);
			}
		}

		public void SpawnObject ()
		{
			for (int i = 0; i < numberToSpawn; i++) {
				Vector3 randomVector = Vector3.zero;

				if (numberToSpawn > 1) {
					randomVector = transform.position.RandomVector (-0.4f, 0.4f);
				} else {
					randomVector = transform.position;
				}

				ObjectManager.instance.AddObject (prefabToSpawn.name, randomVector, Quaternion.identity);
			}
		}
	}
}
