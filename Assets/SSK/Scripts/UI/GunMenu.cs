using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class GunMenu : MonoBehaviour
	{
		public Transform ContentPanel;
		public GameObject ButtonPrefab;
		public GameObject WeaponCratePrefab;
		public MenuItem[] MenuItems;
		public GameObject SuppliesText;

		public Color ButtonColourWhenCannotAfford;
	
		private SelectableTile centreTile;
		private Dictionary<MenuItem, Image> menuImages = new Dictionary<MenuItem, Image> ();

		private static readonly int SPAWN_AREA_SIZE = 7;

		void Start ()
		{
			PopulateList ();

			HideMenu ();
		}
	
		public void PopulateList ()
		{ 
			foreach (var item in MenuItems) {
				var newButton = (GameObject)Instantiate (ButtonPrefab);
				
				var button = newButton.GetComponent<MenuButton> ();
				button.ImageLabel.sprite = item.MenuImage;
				button.NameLabel.text = item.Name;
				button.CostLabel.text = "Cost: " + item.BuildCost;
				button.SampleButton.onClick = item.OnClick;
				newButton.transform.SetParent (ContentPanel);
				
				menuImages.Add (item, newButton.GetComponent<Image> ());
			}
		}

		public void ShowMenu ()
		{
			gameObject.SetActive (true);
			
			foreach (var item in MenuItems) {
				var buttonColour = menuImages [item];
				
				if (item.BuildCost > CashTotal.instance.CurrentCash) {
					buttonColour.color = ButtonColourWhenCannotAfford;
				}
			}
		}
		
		public void HideMenu ()
		{
			gameObject.SetActive (false);
		}

		public void SpawnWeapon (int index)
		{
			var buildCost = MenuItems [index].BuildCost;
			
			if (buildCost > CashTotal.instance.CurrentCash) {
				return;
			}
			
			if (centreTile == null) {
				SetCentreTile ();
			}
		
			var locations = FloorManager.instance.GetNeighbouringFreeTiles (centreTile.Coordinates, new Vector2i (SPAWN_AREA_SIZE, SPAWN_AREA_SIZE));

			if (locations.Count == 0) {
				Debug.Log ("No spawn locations found");
				return;
			}

			EnableSuppliesTextForSeconds (1f);

			var location = locations [Random.Range (0, locations.Count)];

			var crate = ObjectManager.instance.AddObject (WeaponCratePrefab.name, location.transform.position);
			var weaponCrate = crate.GetComponent<WeaponCrate> ();
			weaponCrate.prefabToSpawn = MenuItems [index].WeaponPrefab;
			weaponCrate.Owner = location;
			
			CashTotal.instance.Decrement (buildCost);
			
			HideMenu ();
		}

		private void EnableSuppliesTextForSeconds (float seconds)
		{
			SuppliesText.SetActive (true);
			Invoke ("DisableSuppliesText", seconds);
		}

		private void DisableSuppliesText ()
		{
			SuppliesText.SetActive (false);
		}
		
		private void SetCentreTile ()
		{
			var centre = new Vector2i ((int)FloorManager.instance.RoomSize.width / 2, (int)FloorManager.instance.RoomSize.height / 2);
			centreTile = FloorManager.instance.GetFloorTile (centre);
		}
	}
}
