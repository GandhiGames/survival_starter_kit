using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	[System.Serializable]
	public class RoundSpawnableObjectList
	{
		public SpawnableObject[] spawnables;

		private List<SpawnableObject> _workingSet = new List<SpawnableObject> ();
		public List<SpawnableObject> workingSet {
			get {
				if (_isDirty) {
					UpdateAll ();
				}

				return _workingSet;
			}
		}

		private float _totalWeight;
		public float totalWeight {
			get {
				if (_isDirty) {
					UpdateAll ();
				}

				return _totalWeight;
			}
		}

		public int weightAdjustedIndex {
			get {
				if (_isDirty) {
					UpdateAll ();
				}

				return GetIndex ();
			}
		}

		public GameObject weightAdjustedObject {
			get {

				var index = weightAdjustedIndex;

				if (index == 0 && _workingSet.Count == 0)
					return null;

				return _workingSet [weightAdjustedIndex].prefab;
			}
		}

		private bool _isDirty = true;

		public void MarkDirty ()
		{
			_isDirty = true;
		}
	

		private void UpdateAll ()
		{	
			CalculateWorkingSet ();
			UpdateWeights ();
			CalculateTotalWeight ();
			_isDirty = false;
		}

		private void CalculateWorkingSet ()
		{
			for (int i = 0; i < spawnables.Length; i++) {
	
				if ((spawnables [i].firstSpawnAtRound == RoundDirector.instance.currentRoundNumber) || 
					(RoundDirector.instance.currentRoundNumber == 1 && spawnables [i].firstSpawnAtRound == 0)) {
					_workingSet.Add (spawnables [i]);
				}
			}
		}

		private void UpdateWeights ()
		{
			foreach (var obj in _workingSet) {
				obj.spawnWeights.CalculateCurrent ();
			}
		}

		private void CalculateTotalWeight ()
		{
			_totalWeight = 0f;

			foreach (var obj in _workingSet) {
				_totalWeight += obj.spawnWeights.current;
			}
		}

		private int GetIndex ()
		{
			if (_workingSet.Count <= 1) {
				return 0;
			}
			
			var randomIndex = -1;
			var random = Random.value * _totalWeight;
			
			for (int i = 0; i < _workingSet.Count; ++i) {
				random -= _workingSet [i].spawnWeights.current;
				
				if (random <= 0f) {
					randomIndex = i;
					break;
				}
			}
			
			return randomIndex;
		}

	}
}
