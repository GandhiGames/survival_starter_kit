using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class MenuEnemySpawner : MonoBehaviour
	{
		public GameObject enemyPrefab;

		public Rect spawnBox;

		public float spawnTime = 5f;

		private float _currentTime;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
			_currentTime += Time.deltaTime;

			if (_currentTime > spawnTime) {
				_currentTime = 0f;

				var x = Random.Range (spawnBox.xMin, spawnBox.xMax);
				var y = Random.Range (spawnBox.yMin, spawnBox.yMax);

				var position = new Vector2 (x, y);

				var rot = Quaternion.identity;
				var randRot = new Quaternion (rot.x, rot.y, Random.rotation.z, rot.w);
				
				Instantiate (enemyPrefab, position, randRot);
			}
		}
	}
}
