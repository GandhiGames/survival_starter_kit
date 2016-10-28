using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public struct ComputerSpawnableObject
	{
		public enum SpawnableObjectType
		{
			Wall,
			Weapon
		}
		public SpawnableObjectType objectType;
		public string friendlyName;
		public GameObject prefab;
		public int moneyToUnlock;
	
	}
}
