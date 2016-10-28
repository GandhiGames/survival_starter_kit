using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class EnemySummonerAI : EnemyAI
	{
		public GameObject EnemyPrefabToSummon;
		public float SummonRange = 5f;
		public float TimeBetweenSummons = 5f;
		private GameObject currentTile;

		private int summonHash = Animator.StringToHash ("summoning");
	

		private float currentSummonTime = 0f;

		public override void Awake ()
		{
			base.Awake ();
			canMove = true;

			currentTarget = _player;
		}

		void OnEnable ()
		{
			currentSummonTime = TimeBetweenSummons;
		}

		// Update is called once per frame
		public override void Update ()
		{
			currentSummonTime += Time.deltaTime;

			if (!canMove) {
				animator.SetBool (attackHash, false);
				return;
			}
			
			if (!ObstacleBlockingPath () && !TargetInRange ()) {
				SetTarget ();
			}


			if (OkToSummon () && InSummonRange ()) {
				canMove = false;
				animator.SetBool (summonHash, true);
				return;
			} 

			
			if (currentTarget == null) {
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


		private bool InSummonRange ()
		{
			return (_player.transform.position - transform.position).sqrMagnitude <= (SummonRange * SummonRange);
		}

		private bool OkToSummon ()
		{
			if (currentSummonTime >= TimeBetweenSummons) {
				currentSummonTime = 0f;
				return true;
			}

			return false;
		}

		public void Summon ()
		{
			if (currentTile == null)
				return;

			var tile = currentTile.GetComponent<SelectableTile> ();
			var currentCoord = tile.Coordinates;


			var coords = new Vector2i[4];

			coords [0] = new Vector2i (currentCoord.X - 2, currentCoord.Y);  // left
			coords [1] = new Vector2i (currentCoord.X, currentCoord.Y + 2); // above
			coords [2] = new Vector2i (currentCoord.X + 2, currentCoord.Y); // right
			coords [3] = new Vector2i (currentCoord.X, currentCoord.Y - 2); //bottom

			//coords [4] = new Vector2i (currentCoord.X - 2, currentCoord.Y + 2);  // left above
			//coords [5] = new Vector2i (currentCoord.X + 2, currentCoord.Y + 2); // right above
			//coords [6] = new Vector2i (currentCoord.X + 2, currentCoord.Y - 2); // right bottom
			//coords [7] = new Vector2i (currentCoord.X - 2, currentCoord.Y - 2); //left bottom


			foreach (var c in coords)
				EnemySpawnManager.instance.SpawnEnemy (EnemyPrefabToSummon, c);

			animator.SetBool (summonHash, false);
			canMove = true;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.gameObject.CompareTag ("Tile") && !IsEqual (other.gameObject)) {
				currentTile = other.gameObject;
			}
		}



		private bool IsEqual (GameObject obj)
		{
			if (currentTile == null)
				return false;

			return obj.gameObject.GetInstanceID () == currentTile.GetInstanceID ();
		}
	}
}
