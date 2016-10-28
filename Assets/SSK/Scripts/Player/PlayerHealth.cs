using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SurvivalKit
{
	public class PlayerHealth : MonoBehaviour
	{
		public GameObject OnDeadAnimationPrefab;
		public GameObject[] OnDeadAnimations;
		public GameObject[] OnDamageAnimations;
		public int MaxHealth = 20;
		private int currentHealth;
		public Slider HealthBar;
		public GameOverHandler gameOver;
		
		
		void OnEnable ()
		{
			currentHealth = MaxHealth;
			HealthBar.minValue = 0f;
			HealthBar.maxValue = MaxHealth;
			HealthBar.value = MaxHealth;
		}
	
		public void ApplyDamage (int damage)
		{
			currentHealth -= damage;

			HealthBar.value = currentHealth;

			if (OnDamageAnimations.Length > 0)
				ObjectManager.instance.AddObject (OnDamageAnimations [Random.Range (0, OnDamageAnimations.Length)].name, transform.position, Quaternion.identity);

			if (currentHealth <= 0) {
				OnDead ();
			}
		}

		public void AddHealth (int health)
		{
			currentHealth += health;

			HealthBar.value = currentHealth;

			if (currentHealth > MaxHealth) {
				currentHealth = MaxHealth;
			}
		}
	
		private void OnDead ()
		{
			Instantiate (OnDeadAnimationPrefab, transform.position, Quaternion.identity);

			gameOver.OnGameOver ();

			gameObject.SetActive (false);

		}
		

		
	}
}
