using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class GunBody : GunComponent
	{
		public enum Gun_Position
		{
			RIGHT_ONE_HANDED,
			RIGHT_DUAL_WIELD,
			LEFT_DUAL_WEILD,
			BOTH_TWO_HANDED
		}

		public Gun_Position GunPosition;
		private Vector2 localPosition;
		private SpriteRenderer spriteRenderer;
		private int initialSpriteOrder;

		void Awake ()
		{
			spriteRenderer = GetComponent<SpriteRenderer> ();
			initialSpriteOrder = spriteRenderer.sortingOrder;
			spriteRenderer.sortingOrder = 0;
			localPosition = GetLocalPosition ();
		}

		private Vector2 GetLocalPosition ()
		{
			switch (GunPosition) {
			case Gun_Position.RIGHT_ONE_HANDED:
				return new Vector2 (0.269f, -0.052f);
			case Gun_Position.RIGHT_DUAL_WIELD:
				return new Vector2 (0.244f, -0.07f);
			case Gun_Position.LEFT_DUAL_WEILD:
				return new Vector2 (0.266f, 0.068f);
			case Gun_Position.BOTH_TWO_HANDED:
				return new Vector2 (0.206f, -0.046f);

			}

			return Vector2.zero;
		}
	

		public override void OnPickup ()
		{
			transform.localPosition = localPosition;
			spriteRenderer.sortingOrder = initialSpriteOrder;
		}

		public override void OnDrop ()
		{
			spriteRenderer.sortingOrder = 0;
		}
	}
}
