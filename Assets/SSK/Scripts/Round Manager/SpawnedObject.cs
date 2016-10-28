using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class SpawnedObject : MonoBehaviour
	{

		void OnDisable ()
		{
			RoundDirector.instance.currentRound.EnemyKilled ();
		}
	}
}
