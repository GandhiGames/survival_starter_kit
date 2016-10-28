using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class SimpleHealth : MonoBehaviour
	{
		public int maxHealth = 20;
		private int _currentHealth;
		public GameObject explosionPrefab;
		public bool poolObject;
		public GameObject objectToDestroyOnDeath;
		
		void OnEnable ()
		{
			_currentHealth = maxHealth;
		}
		
		public void ApplyDamage (int damage)
		{
			_currentHealth -= damage;
			
			if (_currentHealth <= 0) {
				OnDead ();
			}
		}
		
		private void OnDead ()
		{
			Instantiate (explosionPrefab, transform.position, Quaternion.identity);

			if (poolObject) {
				ObjectManager.instance.RemoveObject (objectToDestroyOnDeath);
			} else {
				Destroy (objectToDestroyOnDeath);
			}
		}
	}
}
