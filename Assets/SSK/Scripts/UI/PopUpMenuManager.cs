using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PopUpMenuManager : MonoBehaviour
	{
		public PopUpMenu PopupMenu;
		public Transform Canvas;

		public int cheapestPopupMenuItem {
			get {
				return popupMenus [0].cheapestItemValue;
			}
		}

		private PopUpMenu[] popupMenus = new PopUpMenu[2];
		private int currentMenu = 0;

		private static PopUpMenuManager _instance;
		public static PopUpMenuManager instance { get { return _instance; } }

		void Awake ()
		{
			_instance = this;
			popupMenus [0] = PopupMenu;
			popupMenus [1] = Instantiate (PopupMenu);
			popupMenus [1].transform.SetParent (Canvas);
		}

		public void Show (SelectableTile tile)
		{
			popupMenus [currentMenu].Hide ();
			currentMenu = (currentMenu + 1) % popupMenus.Length;
			popupMenus [currentMenu].Show (tile);
		}

		public void Hide ()
		{
			popupMenus [currentMenu].Hide ();
			currentMenu = (currentMenu + 1) % popupMenus.Length;
		}

		public bool IsMenuVisible ()
		{
			return popupMenus [0].IsMenuVisible () || popupMenus [1].IsMenuVisible ();
		}

		public void HideAll ()
		{
			for (int i = 0; i < popupMenus.Length; i++)
				popupMenus [i].Hide ();
		}



	}
}
