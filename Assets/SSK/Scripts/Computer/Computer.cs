using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class Computer : MonoBehaviour
	{
		public GameObject inputText;
		public Text moneyText;
		public UnlockText unlockText;
		public SimpleText nextUnlockText;
		//public GameObject simpleCollectiblePrefab;

		public WallBuilder wallBuilder;

		public ComputerSpawnableObject[] unlockableObjects;
		private List<ComputerSpawnableObject> unlockableObjectsWorkingSet = new List<ComputerSpawnableObject> ();

		public GameObject weaponCratePrefab;

		public Vector2i WeaponSpawnArea = new Vector2i (7, 7);

		public int currentMoney { get; set; }

		private bool inputVisible { get { return inputText.activeSelf; } }
		private bool moneyTextVisible { get { return moneyText.gameObject.activeSelf; } }

		private bool inCentre;

		private float _totalWeight;

		private SelectableTile _currentTile;

		void Start ()
		{
			UpdateNextUnlockText ();
		}
			
		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Player") && CashTotal.instance.CurrentCash > 0) {
				//UpdateMoney ();
				ShowInput ();
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (other.CompareTag ("Player")) {
				HideInput ();
			}
		}

		void Update ()
		{
			if (!inCentre) {
				PlaceInCentre ();
				inCentre = true;
			}

			if (inputVisible && Input.GetButtonUp ("Action")) {
				UpdateMoney ();
			}
		}

		void LateUpdate ()
		{
			if (inputVisible) {
				inputText.transform.position = Camera.main.WorldToScreenPoint (transform.position);
			}

			if (moneyTextVisible) {
				moneyText.transform.position = Camera.main.WorldToScreenPoint (transform.position);
			}
		}

		void OnDestroy ()
		{
			if (inputText != null)
				HideInput ();
		}


		private ComputerSpawnableObject FindNextUnlock ()
		{
			int count = int.MaxValue;
			ComputerSpawnableObject nextUnlock = default (ComputerSpawnableObject);

			foreach (var unlock in unlockableObjects) {
				if (!unlockableObjectsWorkingSet.Contains (unlock)) {
					if (unlock.moneyToUnlock < count) {
						nextUnlock = unlock;
						count = unlock.moneyToUnlock;
					}
				}
			}

			return nextUnlock;
		}

		private void SpawnWeapon (GameObject obj)
		{
			var locations = FloorManager.instance.GetNeighbouringFreeTiles (_currentTile.Coordinates, WeaponSpawnArea);
			
			if (locations.Count == 0) {
				Debug.Log ("No spawn locations found");
				return;
			}
			var location = locations [Random.Range (0, locations.Count)];
			
			var crate = ObjectManager.instance.AddObject (weaponCratePrefab.name, location.transform.position);
			var weaponCrate = crate.GetComponent<WeaponCrate> ();
			weaponCrate.prefabToSpawn = obj;
			weaponCrate.Owner = location;
		}



		private void UpdateMoney ()
		{
			int moneyToAdd = CashTotal.instance.CurrentCash;

			if (moneyToAdd <= 0)
				return;

			HideInput ();

			CashTotal.instance.Decrement (moneyToAdd);

			currentMoney += moneyToAdd;

			ShowMoneyText (moneyToAdd);

			if (CheckForObjectUnlock ()) {
				UpdateNextUnlockText ();
			}

		}

		public void ShowMoneyText (int amount = 1)
		{
			moneyText.gameObject.SetActive (true);
			moneyText.text = "" + amount;

		}

		private void UpdateNextUnlockText ()
		{
			ComputerSpawnableObject nextUnlock = FindNextUnlock ();
			
			if (!nextUnlock.Equals (default (ComputerSpawnableObject))) {
				nextUnlockText.UpdateMessage ("Next Unlock: " + nextUnlock.friendlyName);
			} else {
				nextUnlockText.UpdateMessage ("");
			}
		}


		private bool CheckForObjectUnlock ()
		{
			bool objUnlocked = false;

			foreach (var obj in unlockableObjects) {
				if (obj.moneyToUnlock <= currentMoney && !unlockableObjectsWorkingSet.Contains (obj)) {
					unlockableObjectsWorkingSet.Add (obj);
					
					if (obj.objectType == ComputerSpawnableObject.SpawnableObjectType.Weapon) {
						unlockText.AddMessage (new Message ("[Weapon Unlocked: " + obj.friendlyName + "]", 1f));
						SpawnWeapon (obj.prefab);
					} else if (obj.objectType == ComputerSpawnableObject.SpawnableObjectType.Wall) {
						unlockText.AddMessage (new Message ("[" + obj.friendlyName + " Unlocked]", 1f));
						wallBuilder.BuildWall (obj.prefab.name);
					}

					objUnlocked = true;
				}
			}

			return objUnlocked;
		}

/*		private IEnumerator CreateSimpleCollectible (int moneyToAdd)
		{
			for (int i = 0; i < moneyToAdd; i++) {
				var simpleCollectible = ObjectManager.instance.AddObject (simpleCollectiblePrefab.name, CashTotal.instance.screenPosition);
				simpleCollectible.GetComponent<SimpleCollectible> ().owner = this;
				CashTotal.instance.Decrement ();
				yield return new WaitForSeconds (0.05f);
			}


		}*/


		private void ShowInput ()
		{
			inputText.SetActive (true);
		}
		
		private void HideInput ()
		{
			inputText.SetActive (false);
		}

		private void PlaceInCentre ()
		{
			_currentTile = FloorManager.instance.GetCentreTile ();
			_currentTile.Selectable = false;
			transform.position = _currentTile.transform.position;
		}


	}
}
