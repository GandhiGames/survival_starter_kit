using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SurvivalKit
{
	[System.Serializable]
	public class MenuItem
	{
		public GameObject WeaponPrefab;
		public Sprite MenuImage;
		public string Name;
	
		public int BuildCost = 1;

		public Button.ButtonClickedEvent OnClick;
	
	}
}
