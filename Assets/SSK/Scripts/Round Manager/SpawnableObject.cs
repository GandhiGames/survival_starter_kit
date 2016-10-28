using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public struct SpawnableObject
	{

		public GameObject prefab;

		public int firstSpawnAtRound;
		public RoundPropertyf spawnWeights;
	
	}
}
