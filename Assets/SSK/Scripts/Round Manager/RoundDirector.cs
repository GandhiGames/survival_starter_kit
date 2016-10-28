using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class RoundDirector : MonoBehaviour
	{
		[Header ("Text")]
		public PreperationText
			preperationCountdownText;
		public RoundText suppliesText;
		public float timeToShowSuppliesText;
		public RoundText roundStartText;
		public float timeToShowRoundStartText;

		[Header ("Preperation")]
		public RoundPropertyf
			preperationTime;
		public RoundSpawnableObjectList objectsToSpawnDuringPreperation;
		public int minPrepObjectCount;
		public int maxPrepObjectCount;
		public int prepCountToSpawn { 
			get { 
				return Random.Range (minPrepObjectCount, maxPrepObjectCount); 
			} 
		}

		[Header ("Round enemies")]
		public RoundSpawnableObjectList
			roundEnemies;
		public RoundPropertyi enemyCount;
		public RoundPropertyf timeBetweenSpawns;
		public float maximumTimeOffset;
		public float spawnTime {
			get {
				timeBetweenSpawns.CalculateCurrent ();
				return Random.Range (timeBetweenSpawns.current, timeBetweenSpawns.current + maximumTimeOffset);
			}
		}

		[Header ("Misc prefabs")]
		public GameObject
			weaponCratePrefab;

		private int _currentRoundNumber;
		public int currentRoundNumber { get { return _currentRoundNumber; } }

		private Round _currentRound;
		public Round currentRound { get { return _currentRound; } }

		public delegate IEnumerator RoutineEvent ();

		private static RoundDirector _instance;
		public static RoundDirector instance {
			get {
				return _instance;
			}
		}

		void Awake ()
		{
			_instance = this;
		}


		// Use this for initialization
		void Start ()
		{
			StartNewRound ();
		}


		void Update ()
		{
			if (_currentRound != null) {
				_currentRound.Update ();
			}
		}

		public void StartNewRound ()
		{
			_currentRoundNumber++;

			MarkSpawnablesDirty ();

			if (_currentRound != null) {
				_currentRound.Restart ();
			} else {
				_currentRound = new Round (this);
			}
		}

		public void StartEvent (RoutineEvent e)
		{
			StartCoroutine (e ());
		}

		public void ShowText (RoundText textObj, string text, float seconds, RoundText.delegateFunc func = null)
		{
			if (textObj != null) {
				textObj.ShowForSeconds (seconds, text, func);
			}
		}

		public void ShowPreperationText (Preperation preperation, RoundText.delegateFunc func = null)
		{
			if (preperationCountdownText != null) {
				preperationCountdownText.StartCountdown (preperation, func);
			}
		}

		private void MarkSpawnablesDirty ()
		{
			roundEnemies.MarkDirty ();
			objectsToSpawnDuringPreperation.MarkDirty ();
		}

	}
}
