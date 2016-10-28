using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Collider2D))]
	public class MissileExplosion : MonoBehaviour
	{
		public float PushBackForce = 10f;

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Enemy")) {
				var otherRigidbody = other.GetComponent<Rigidbody2D> ();
			
				if (otherRigidbody) {
					var heading = other.transform.position - transform.position;
					var distance = heading.magnitude;
					var dir = heading / distance;
				
					otherRigidbody.AddForce (dir * PushBackForce);
				
				}
			}
		}
	}
}
