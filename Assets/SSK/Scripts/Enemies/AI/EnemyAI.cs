using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	[RequireComponent (typeof(Animator))]
	[RequireComponent (typeof(Rigidbody2D))]
	public class EnemyAI : MonoBehaviour
	{
		public float MoveSpeed;
		public LayerMask ObstacleLayers;
		public int DamageAmount = 2;
		public float SightRadius = 5f;
		public LayerMask TargetLayers;
		public float AttackDistance = 0.5f;

		protected Transform currentTarget;

		protected Transform _player;

		protected bool canMove;
		public bool CanMove {
			set {
				canMove = value;
			}
		}
		
		protected Animator animator;
		protected int attackHash = Animator.StringToHash ("attacking");

		private float _currentSeekTargetTime;
		private float _currentExecutionLimit;

		private static readonly float MAX_SEEK_TARGET_TIME = 1.2f;
		private static readonly float EXECUTION_LIMIT = 50;


		public virtual void Awake ()
		{
			animator = GetComponent<Animator> ();
			canMove = false;
			var player = GameObject.FindGameObjectWithTag ("Player");

			if (player) {
				currentTarget = player.transform;
				_player = currentTarget;
			}
		}


		protected void SetTarget ()
		{
			var halfSight = SightRadius * .5f;

			if ((_player.transform.position - transform.position).sqrMagnitude < (halfSight * halfSight)) {
				currentTarget = _player;
			}

			var targets = Physics2D.OverlapCircleAll (transform.position, SightRadius, TargetLayers);

			if (targets.Length == 0) {
				currentTarget = _player;
				return;
			}

			foreach (var t in targets) {
				if (t.gameObject.transform == _player) {
					currentTarget = _player;
					return;
				}
			}

			Transform closest = null;
			float distance = float.MaxValue;
			
			foreach (var t in targets) {
				var dis = (t.transform.position - transform.position).sqrMagnitude;
				
				if (dis < distance * distance) {
					distance = dis;
					closest = t.transform;
				}
			}

			if (closest) {
				currentTarget = closest;
			}
		}

		public void SpawnComplete ()
		{
			canMove = true;
		}

		public virtual void Update ()
		{
			if (_player == null)
				return;

			_currentSeekTargetTime += Time.deltaTime;
			_currentExecutionLimit += Time.deltaTime;

			if (!canMove) {
				animator.SetBool (attackHash, false);
				return;
			}

		

			if (!ObstacleBlockingPath () && (NoTarget () || _currentExecutionLimit >= EXECUTION_LIMIT)) {
				_currentExecutionLimit = 0;
				SetTarget ();
			}
			
			if (NoTarget ()) {
				animator.SetBool (attackHash, false);
				return;
			}
		
			if (InAttackRange ()) {
				animator.SetBool (attackHash, true);
			} else {
				animator.SetBool (attackHash, false);
				MoveToTarget ();
			}
			
		}

		private bool NoTarget ()
		{
			return currentTarget == null || !currentTarget.gameObject.activeSelf;
		}

		protected bool ObstacleBlockingPath ()
		{
			var hit = Physics2D.Raycast (transform.position, transform.right, 0.9f, ObstacleLayers);

			if (hit.collider != null) {
				currentTarget = hit.transform;
				return true;
			}

			return false;
		}
	
		protected bool InAttackRange ()
		{
			return (currentTarget.position - transform.position).sqrMagnitude <= (AttackDistance * AttackDistance);
		}
		
		protected void MoveToTarget ()
		{
			var heading = currentTarget.position - transform.position;
			var distance = heading.magnitude;
			var dir = heading / distance;
			
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.Translate (dir * MoveSpeed * Time.deltaTime, Space.World);
		}
		
		public void ApplyDamageToTarget ()
		{
			if (currentTarget) {
				currentTarget.SendMessage ("ApplyDamage", DamageAmount);
			}
		}

		private bool OkToSearchForTarget ()
		{
			return _currentSeekTargetTime >= MAX_SEEK_TARGET_TIME;
		}

		protected bool TargetInRange ()
		{
			if (NoTarget ())
				return false;


			var dis = (currentTarget.position - transform.position).sqrMagnitude;
			
			return dis < SightRadius * SightRadius;
		}
	}
}
