using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public class PopupMenuItem
	{
		public GameObject MenuItemPrefab;
		public Sprite PopUpMenuImage;

		public string Name;
	
		public float BuildTime = 1f;
		public int BuildCost = 1;

		public Button.ButtonClickedEvent OnClick;

		public enum ObjectType
		{
			Wall,
			Turret
		}
		public ObjectType objectType;
	}
}