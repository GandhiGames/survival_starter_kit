using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class TurretHealth : MonoBehaviour
	{

		public int MaxHealth = 20;
		private int currentHealth;
		public GameObject ExplosionPrefab;

		private SelectableTile owner;
		public SelectableTile Owner { set { owner = value; } }

		private bool takeDamage;
		public bool TakeDamage { set { takeDamage = value; } }

		void OnEnable ()
		{
			currentHealth = MaxHealth;
			takeDamage = false;
		}

		public void ApplyDamage (int damage)
		{
			if (!takeDamage)
				return;

			currentHealth -= damage;
		
			if (currentHealth <= 0) {
				OnDead ();
			}
		}
	
		public void OnDead ()
		{
			Instantiate (ExplosionPrefab, transform.position, Quaternion.identity);
			owner.Selectable = true;
			ObjectManager.instance.RemoveObject (gameObject);
		}
	}
}
