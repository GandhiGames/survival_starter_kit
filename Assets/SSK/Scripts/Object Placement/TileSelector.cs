using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SurvivalKit
{
	public class TileSelector : MonoBehaviour
	{
		public PopUpMenuManager menu;
		public LayerMask TileLayer;
		public Text cannotAffordText;
		private List<SelectableTile> tiles = new List<SelectableTile> ();

		private bool selectableTilesShown = false;
		private bool menuOpen = false;
		public bool TileSelectionInProgress { get { return selectableTilesShown || menuOpen; } }

		private SelectableTile _selectedTile;

		private static TileSelector _instance;
		public static TileSelector instance { get { return _instance; } }

		void Awake ()
		{
			_instance = this;
		}
	

		// Update is called once per frame
		void Update ()
		{
			if (CashTotal.instance.CurrentCash < menu.cheapestPopupMenuItem) {
				if (Input.GetButtonUp ("HighlightTiles")) {
					if (cannotAffordText) {
						StartCoroutine (UpdateTextForSeconds ("Cannot Afford Turrets"));
					}
				}

				if (CashTotal.instance.CurrentCash == 0) {
					
					if (menu.IsMenuVisible ()) {
						menu.HideAll ();
						
						if (_selectedTile != null) {
							_selectedTile.ShowNormal ();
						}
					}
				}
				return;
			}

	

			if (Input.GetButtonUp ("HighlightTiles")) {

				if (!menu.IsMenuVisible ()) {

					if (!selectableTilesShown) {
						foreach (var tile in tiles) {
							tile.ShowSelectable ();
						}
						selectableTilesShown = true;
					} else {
						foreach (var tile in tiles) {
							tile.ShowNormal ();
						}
						selectableTilesShown = false;
					}
				} else {
					menu.HideAll ();

					if (_selectedTile != null) {
						_selectedTile.ShowNormal ();
					}
				}
			} 

		

			if (selectableTilesShown && Input.GetMouseButtonDown (0)) {

				var hit = GetTileHit ();

				if (hit.collider != null) {
					_selectedTile = hit.collider.GetComponent<SelectableTile> ();
					if (_selectedTile != null && _selectedTile.Selectable) {
						ShowPlacementMenu (_selectedTile);
						HideOtherSelectableTiles (_selectedTile);
						selectableTilesShown = false;
					}
				}
			} else if (Input.GetMouseButtonDown (0) && menu.IsMenuVisible ()) {
				//	menu.RegisterMenuToClose ();
				menu.HideAll ();

				if (_selectedTile != null) {
					_selectedTile.ShowNormal ();
				}
			}
		}

		private void HideOtherSelectableTiles (SelectableTile tileToKeep)
		{
			foreach (var tile in tiles) {
				if (!tile.Equals (tileToKeep)) {
					tile.ShowNormal ();
				} else {
					tile.ShowSelectable ();
				}
			}
		}

		private RaycastHit2D GetTileHit ()
		{
			var hit = Physics2D.Raycast (
			new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), 
			Vector2.zero, 
			0f, TileLayer);

			if (!hit.collider.gameObject.CompareTag ("Tile"))
				return default (RaycastHit2D);

			return hit;

			/*foreach (var h in hit) {
			if (h.collider.gameObject.layer == 1 << LayerMask.NameToLayer ("Tile")) {
				return h;
			}
		}

		return default (RaycastHit2D);*/
		}

		public void ShowPlacementMenu (SelectableTile tile)
		{
			menu.Show (tile);
		}

		public void RegisterTile (SelectableTile tile)
		{
			tiles.Add (tile);
		}

		private IEnumerator UpdateTextForSeconds (string text)
		{
			string previousText = cannotAffordText.text;

			cannotAffordText.text = text;

			yield return new WaitForSeconds (0.5f);
			cannotAffordText.text = previousText;
		}

	}
}
