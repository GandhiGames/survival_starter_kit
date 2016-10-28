using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class Director : MonoBehaviour
	{
		public GameObject Player;

		// Use this for initialization
		void Start ()
		{
			FloorManager.instance.GenerateFloor ();

			SpawnPlayer ();
		}

		private void SpawnPlayer ()
		{
			var floorManager = FloorManager.instance;

			var spawnCoord = new Vector2i ((int)floorManager.RoomSize.width / 2, (int)floorManager.RoomSize.height / 2);
			
			spawnCoord.Y--;
			
			var spawnTile = floorManager.GetFloorTile (spawnCoord);
			
			Player.transform.position = spawnTile.transform.position;
			Player.SetActive (true);
		}

	}
}
