using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public class PopUpMenu : MonoBehaviour
	{
		public GameObject ButtonPrefab;
		public Transform Canvas;
		public PopupMenuItem[] MenuItems;
		public WallBuilder wallBuilder;

		public float spacing = 12f;
		public float speed = 8f;
	
		public int cheapestItemValue { get; private set; }

		private float scale;

		private SelectableTile currentTile;

		private List<GameObject> buttons = new List<GameObject> ();
		private List<GameObject> activeButtons = new List<GameObject> ();

		private List<RectTransform> buttonRects = new List<RectTransform> ();
		private Dictionary<PopupMenuItem, Image> menuImages = new Dictionary<PopupMenuItem, Image> ();


		void Start ()
		{
			CreateButtons ();
			transform.localScale = Vector3.one;
			foreach (var button in buttons) {
				button.SetActive (false);
			}
		}

		private void ScaleGUI (float s)
		{
			var scale = new Vector3 ((Vector3.one.x * s), (Vector3.one.y * s), 1f);

			transform.localScale = scale;
		}

		private void ScaleAllButtons (Vector3 scale)
		{
			foreach (var button in activeButtons) {
				button.transform.localScale = scale;
			}
		}

		private void UpdateMenu ()
		{

			ScaleGUI (scale);

			float d = (2 * Mathf.PI) / activeButtons.Count;
			float radius = (spacing * activeButtons.Count);

			for (int i = 0; i < activeButtons.Count; i++) {
				var rectTransform = buttonRects [i];
				float theta = (d * i);
				float ix = (Mathf.Cos (theta) * radius) - (rectTransform.rect.width / 2);
				float iy = (Mathf.Sin (theta) * radius) - (rectTransform.rect.width / 2);


				activeButtons [i].transform.localPosition = new Vector3 (ix + rectTransform.rect.width / 2, iy + rectTransform.rect.height, 0f);
			}

		}

		
		public void Show (SelectableTile tile)
		{
			currentTile = tile;
			transform.position = Camera.main.WorldToScreenPoint (currentTile.transform.position);

			scale = 0f;

			UpdateButtonList ();
			StartCoroutine (_Show ());
		
		}

		public void Hide ()
		{
			if (scale > 0) {
				StartCoroutine (_Hide ());
			}
		}

		private IEnumerator _Show ()
		{
			ShowButtons (true);

			while (scale < 1) {
				yield return new WaitForEndOfFrame ();
				scale += Time.deltaTime * speed;
				if (scale > 1)
					scale = 1f;
				UpdateMenu ();
			}
			scale = 1f;
		}

		private IEnumerator _Hide ()
		{
			yield return new  WaitForSeconds (0.2f);

			while (scale > 0) {
				yield return new WaitForEndOfFrame ();
				scale -= Time.deltaTime * speed;
				if (scale < 0)
					scale = 0f;
				UpdateMenu ();
			}
			scale = 0f;

			ShowButtons (false);
		}

		void LateUpdate ()
		{
			if (currentTile != null) {
				transform.position = Camera.main.WorldToScreenPoint (currentTile.transform.position);
			}
		}

		public void SpawnItem (int index)
		{
			var menuitem = MenuItems [index];

			var buildCost = menuitem.BuildCost;
			
			if (!currentTile.Selectable || buildCost > CashTotal.instance.CurrentCash) 
				return;

			if (menuitem.objectType == PopupMenuItem.ObjectType.Turret) {
				SpawnTurret (menuitem);
			} else {
				wallBuilder.BuildWall (menuitem.MenuItemPrefab.name, menuitem.BuildTime);
			}

		}

		public bool IsMenuVisible ()
		{
			if (activeButtons == null || activeButtons.Count == 0)
				return false;

			return activeButtons [0].activeSelf;
		}

		private void SpawnTurret (PopupMenuItem item)
		{
			currentTile.Selectable = false;
			var rot = Quaternion.identity;
			var randRot = new Quaternion (rot.x, rot.y, Random.rotation.z, rot.w);
			var turret = ObjectManager.instance.AddObject (item.MenuItemPrefab.name, currentTile.transform.position, randRot);
			
			var build = turret.GetComponent<Build> ();
			
			if (build)
				build.Initialise (item.BuildTime, currentTile);
			
			CashTotal.instance.Decrement (item.BuildCost);
		}

		private void ShowButtons (bool show)
		{
			foreach (var button in activeButtons) {
				button.SetActive (show);
			}
		}

		private void UpdateButtonList ()
		{
			activeButtons.Clear ();

			for (int i = 0; i < MenuItems.Length; i++) {
				if (MenuItems [i].BuildCost <= CashTotal.instance.CurrentCash) {
					activeButtons.Add (buttons [i]);
				}
			}
		}

		private void CreateButtons ()
		{
			int cheapest = int.MaxValue;

			foreach (var item in MenuItems) {
				var newButton = (GameObject)Instantiate (ButtonPrefab);
				newButton.transform.SetParent (transform);
				newButton.transform.localScale = Vector3.one;
				var button = newButton.GetComponent<PopupMenuButton> ();
				button.ImageLabel.sprite = item.PopUpMenuImage;
				button.NameLabel.text = item.Name;
				button.CostLabel.text = "Cost: " + item.BuildCost;

				if (item.BuildCost < cheapest) {
					cheapest = item.BuildCost;
				}

				/*			if (CashTotal.instance.CurrentCash < item.BuildCost) {
					button.gameObject.GetComponent<Image> ().color = Color.red;
				} else {
					button.gameObject.GetComponent<Image> ().color = Color.white;
				}*/

				button.BuildLabel.text = "Build Time: " + item.BuildTime;
				button.SampleButton.onClick = item.OnClick;
				buttons.Add (newButton);
				
				var rect = newButton.GetComponent<RectTransform> ();
				buttonRects.Add (rect);
				
				menuImages.Add (item, newButton.GetComponent<Image> ());
			}

			cheapestItemValue = cheapest;
		}

	}
}
