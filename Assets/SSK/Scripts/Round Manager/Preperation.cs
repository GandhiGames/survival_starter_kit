using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class Preperation
	{
		public bool isActive { get { return _currentPreperationTime >= 0f; } }

		private Round _round;
		private float _currentPreperationTime;
		public int timeInSeconds { get { return (int)_currentPreperationTime; } }

		private int _objectCountToSpawn;
		private SelectableTile _centreTile;

		private static readonly string SUPPLIES_INCOMING_TEXT = "Supplies Incoming!";
		
		public Preperation (Round round)
		{
			_round = round;
		}

		public void Restart ()
		{
			OnPreperationStart ();
		}

		public void Update ()
		{
			if (!isActive) {
				return;
			}

			_currentPreperationTime -= Time.deltaTime;
		}

		private void SpawnObjects ()
		{	
			Debug.Log ("Spawning object prep count: " + _objectCountToSpawn);
			for (int i = 0; i < _objectCountToSpawn; i++) {
				var objToSpawn = _round.director.objectsToSpawnDuringPreperation.weightAdjustedObject;

				int numToSpawn = objToSpawn.gameObject.CompareTag ("Collectible") ? Random.Range (2, 6) : 1;

				SpawnObject (objToSpawn, numToSpawn);
			}

			_objectCountToSpawn = 0;
		}

		private void SpawnObject (GameObject obj, int numToSpawn = 1)
		{
			var locations = FloorManager.instance.GetNeighbouringFreeTiles (_centreTile.Coordinates, new Vector2i (6, 6));
			
			if (locations.Count == 0) {
				Debug.Log ("No spawn locations found");
				return;
			}
			var location = locations [Random.Range (0, locations.Count)];
			
			var crate = ObjectManager.instance.AddObject (_round.director.weaponCratePrefab.name, location.transform.position);
			var weaponCrate = crate.GetComponent<WeaponCrate> ();
			weaponCrate.prefabToSpawn = obj;
			weaponCrate.Owner = location;
			weaponCrate.numberToSpawn = numToSpawn;
		}


		private void SetCentreTile ()
		{
			var centre = new Vector2i ((int)FloorManager.instance.RoomSize.width / 2, (int)FloorManager.instance.RoomSize.height / 2);
			_centreTile = FloorManager.instance.GetFloorTile (centre);
		}
		
		private void OnPreperationStart ()
		{
			Debug.Log ("Starting preperation");

			SetCentreTile ();
			_currentPreperationTime = _round.director.preperationTime.CalculateAndGetCurrent ();

			_objectCountToSpawn = _round.director.prepCountToSpawn;

			_round.director.ShowPreperationText (this, OnPreperationOver);

			_round.director.StartEvent (ShowSupplyTextAndSpawnSupplies);
		}

		private IEnumerator ShowSupplyTextAndSpawnSupplies ()
		{
			yield return new WaitForSeconds (1f);

			//if (_round.director.objectsToSpawnDuringPreperation.workingSet.Count != 0 && _round.director.computer != null) {
			_round.director.ShowText (_round.director.suppliesText, SUPPLIES_INCOMING_TEXT, _round.director.timeToShowSuppliesText, SpawnObjects);
			//}
		}

		private void OnPreperationOver ()
		{
			_round.OnRoundStart ();
		}
	}
}
