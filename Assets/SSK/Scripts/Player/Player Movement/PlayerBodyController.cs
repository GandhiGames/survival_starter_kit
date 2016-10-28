using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class PlayerBodyController : MonoBehaviour
	{
		public Sprite OneHandedWeaponBody;
		public Sprite TwoHandedWeaponBody;
		public Sprite DualWieldWeaponBody;

		private SpriteRenderer spriteRenderer;

		void Awake ()
		{
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		public void PickedUpOneHanded ()
		{
			spriteRenderer.sprite = OneHandedWeaponBody;
		}

		public void PickedUpTwoHandedWeapon ()
		{
			spriteRenderer.sprite = TwoHandedWeaponBody;
		}

		public void PickedUpDualWieldWeapon ()
		{
			spriteRenderer.sprite = DualWieldWeaponBody;
		}
	}
}
