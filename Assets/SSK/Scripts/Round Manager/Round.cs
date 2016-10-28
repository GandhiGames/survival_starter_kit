using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class Round
	{
		private RoundDirector _director;
		public RoundDirector director { get { return _director; } }

		private List<SpawnableObject> _roundEnemies;
		private int _spawnCount;

		private Preperation _preperation;
		public Preperation preperation { get { return _preperation; } }

		private float _totalWeight;
		private float _currentTimeBetweenSpawn;
		private int _activeEnemyCount;
		private int _enemiesToSpawn;
		private bool _roundStarted;

		private static readonly string ROUND_TEXT_PRE = "Round ";

		public Round (RoundDirector director)
		{
			_director = director;
			_preperation = new Preperation (this);
			Restart ();
		}

		public void Restart ()
		{
			_director.ShowText (_director.roundStartText, ROUND_TEXT_PRE + _director.currentRoundNumber, 
			                    _director.timeToShowRoundStartText, OnRoundEnter);
		}

		public void Update ()
		{
			if (!_roundStarted) {
				_preperation.Update ();
				return;
			}

			_currentTimeBetweenSpawn -= Time.deltaTime;

			if (_currentTimeBetweenSpawn <= 0f && _activeEnemyCount < _director.enemyCount.current && _enemiesToSpawn > 0) {
				SpawnEnemy ();

				if (_enemiesToSpawn > 0) 
					InitialiseSpawnTime ();
			}
		}

		public void EnemyKilled ()
		{
			Debug.Log ("enemy killed");

			if (--_activeEnemyCount <= 0) {
				OnRoundOver ();
			}

		}

		private void SpawnEnemy ()
		{
			int index = GetIndex ();

			Debug.Log ("spawning enemy: " + _roundEnemies [index].prefab.name);

			EnemySpawnManager.instance.SpawnEnemy (_roundEnemies [index].prefab);

			_activeEnemyCount++;
			_enemiesToSpawn--;

			Debug.Log ("Active enemies: " + _activeEnemyCount + " enemies to spawn: " + _enemiesToSpawn);
		}

		private void InitialiseSpawnTime ()
		{
			_currentTimeBetweenSpawn = _director.spawnTime;
			Debug.Log ("Spawn time: " + _currentTimeBetweenSpawn);
		}

		private void OnRoundEnter ()
		{
			Debug.Log ("starting round: " + _director.currentRoundNumber);
			
			_roundEnemies = _director.roundEnemies.workingSet;
			_totalWeight = _director.roundEnemies.totalWeight;

			_spawnCount = 0;

			_enemiesToSpawn = _director.enemyCount.CalculateAndGetCurrent ();

			InitialiseSpawnTime ();

			_roundStarted = false;

			_preperation.Restart ();
		}

		public void OnRoundStart ()
		{
			_roundStarted = true;
		}

		private void OnRoundOver ()
		{
			_director.StartNewRound ();
		}


		private int GetIndex ()
		{
			if (_roundEnemies.Count == 1) {
				return 0;
			}
		
			var randomIndex = -1;
			var random = Random.value * _totalWeight;
		
			for (int i = 0; i < _roundEnemies.Count; ++i) {
				random -= _roundEnemies [i].spawnWeights.current;
			
				if (random <= 0f) {
					randomIndex = i;
					break;
				}
			}

			return randomIndex;
		}

	}
}
