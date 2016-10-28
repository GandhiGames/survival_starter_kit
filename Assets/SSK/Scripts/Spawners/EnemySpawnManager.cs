using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class EnemySpawnManager : MonoBehaviour
	{
		private List<SelectableTile> spawnTiles = new List<SelectableTile> ();
		private List<EnemyHealth> enemies = new List<EnemyHealth> ();

		private static EnemySpawnManager _instance;
		public static EnemySpawnManager instance { get { return _instance; } }

		void Awake ()
		{
			_instance = this;
		}


		public void DestroyEnemies ()
		{
			foreach (var enemy in enemies) {
				if (enemy)
					enemy.OnDead ();
			}

			enemies.Clear ();
		}

		public void SpawnEnemy (GameObject enemyPrefab, Vector2i coord)
		{
			var tile = FloorManager.instance.GetFloorTile (coord);

			if (tile && tile.Selectable) {
				AddEnemy (enemyPrefab.name, tile.transform.position);
			}
		}


		public void SpawnEnemy (GameObject enemyPrefab)
		{
			SelectableTile spawnTile = spawnTiles [Random.Range (0, spawnTiles.Count)];

			/*do {
				spawnTile = spawnTiles [Random.Range (0, spawnTiles.Count)];
			} while (!spawnTile.Selectable)*/
			;

			var pos = spawnTile.transform.position;

			AddEnemy (enemyPrefab.name, pos);
		}

		private void AddEnemy (string prefabName, Vector2 position)
		{
			var enemy = ObjectManager.instance.AddObject (prefabName, position, false);
			enemy.transform.SetParent (transform);
			enemies.Add (enemy.GetComponent<EnemyHealth> ());
		}

		public void RegisterTile (SelectableTile tile)
		{
			spawnTiles.Add (tile);
			tile.transform.SetParent (transform);
		}
		
	}
}
