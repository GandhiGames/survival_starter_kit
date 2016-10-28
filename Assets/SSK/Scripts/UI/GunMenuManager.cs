using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(CircleCollider2D))]
	public class GunMenuManager : MonoBehaviour
	{

		public GunMenu Menu;
		public GameObject InputText;
		public float UseRadius;
		private CircleCollider2D _collider;
		private bool canShowMenu;

		private static GunMenuManager _instance;
		public static GunMenuManager instance { get { return _instance; } }

		// Use this for initialization
		void Awake ()
		{
			_instance = this;
			_collider = GetComponent<CircleCollider2D> ();
			_collider.isTrigger = true;
			_collider.radius = UseRadius;
			HideInput ();
		}

		void Start ()
		{
			var centre = new Vector2i ((int)FloorManager.instance.RoomSize.width / 2, (int)FloorManager.instance.RoomSize.height / 2);
			var centreTile = FloorManager.instance.GetFloorTile (centre);
			centreTile.Selectable = false;
			transform.position = centreTile.transform.position;
		}

	
		// Update is called once per frame
		void Update ()
		{
			if (canShowMenu && Input.GetButtonUp ("Action")) {
				if (MenuVisible ()) {
					HideMenu ();
				} else {
					ShowMenu ();
				}
			}

			/*if (MenuVisible () && Input.GetMouseButtonUp (0)) {
				HideMenu ();
			}*/
		}

		void LateUpdate ()
		{
			if (InputVisible ()) {
				InputText.transform.position = Camera.main.WorldToScreenPoint (transform.position);
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Player")) {
				ShowInput ();
				canShowMenu = true;
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (other.CompareTag ("Player")) {
				HideInput ();
				HideMenu ();
				canShowMenu = false;
			}
		}

		private void ShowInput ()
		{
			InputText.SetActive (true);
		}

		private void HideInput ()
		{
			InputText.SetActive (false);
		}

		private void ShowMenu ()
		{
			Menu.ShowMenu ();
		}

		private void HideMenu ()
		{
			Menu.HideMenu ();
		}

		public bool MenuVisible ()
		{
			return Menu.gameObject.activeSelf;
		}

		private bool InputVisible ()
		{
			return InputText.activeSelf;
		}
	}
}
