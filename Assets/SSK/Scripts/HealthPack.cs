using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class HealthPack : MonoBehaviour
	{
		public int HealAmount = 10;


		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.gameObject.CompareTag ("Player")) {
				var playerHealth = other.gameObject.GetComponent<PlayerHealth> ();

				if (playerHealth) {
					playerHealth.AddHealth (HealAmount);
				}

				ObjectManager.instance.RemoveObject (gameObject);
			}
		}

	}
}
