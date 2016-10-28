using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class MachineGun : DamageTurret
	{
		private Animator _animator;
		private int firingHash = Animator.StringToHash ("firing");

		public override void Awake ()
		{
			_animator = GetComponent<Animator> ();
			_animator.speed = ShootSpeed;
			base.Awake ();
		}
	
		void Update ()
		{
			if (NoTargets ()) {
				_animator.SetBool (firingHash, false);
				return;
			}
		
			if (TargetDestroyed ()) {
				_animator.SetBool (firingHash, false);
				GetTarget ();
				return;
			}

			bool facingTarget = RotateTowardsTarget ();
		
			if (facingTarget) {
				_animator.SetBool (firingHash, true);
			} else {
				_animator.SetBool (firingHash, false);
			}
		}

	}
}
