using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Animator))]
	public class GunMuzzle : GunComponent
	{
		public float FlashAnimationSpeed = 1f;

		private Animator animator;

		void Awake ()
		{
			animator = GetComponent<Animator> ();
			animator.speed = FlashAnimationSpeed;

			gameObject.SetActive (false);
		}

		public override void OnPickup ()
		{

		}

		public override void OnDrop ()
		{
			gameObject.SetActive (false);
		}
	}
}
