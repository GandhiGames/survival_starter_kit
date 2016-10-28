using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class Wall : MonoBehaviour
	{
		public int Health = 5;
		public GameObject WallDestroyedAnimationPrefab;
		public Vector2i Coordinates { get; set; }
		public SelectableTile Owner { get; set; }

		private int currentHealth;

		public Sprite Wall_North_West;
		public Sprite Wall_North;
		public Sprite Wall_North_East;
		public Sprite Wall_East;
		public Sprite Wall_South_East;
		public Sprite Wall_South;
		public Sprite Wall_South_West;
		public Sprite Wall_West;

		private SpriteRenderer _renderer;
		private bool initialised;
		public bool Initialised { set { initialised = value; } }

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		void OnEnable ()
		{
			currentHealth = Health;
			initialised = false;
		}

		public void ApplyDamage (int amount)
		{
			if (!initialised) {
				return;
			}
			currentHealth--;

			if (currentHealth <= 0) {
				Destroy ();
			}
		}

		public void Destroy ()
		{
			Owner.Selectable = true;
			Instantiate (WallDestroyedAnimationPrefab, transform.position, Quaternion.identity);
			ObjectManager.instance.RemoveObject (gameObject);
		}

		public void Orientate ()
		{
			var wallAbove = WallManager.instance.HasNeighbour (Coordinates, Direction.ABOVE);
			var wallBelow = WallManager.instance.HasNeighbour (Coordinates, Direction.BELOW);
			var wallLeft = WallManager.instance.HasNeighbour (Coordinates, Direction.LEFT);
			var wallRight = WallManager.instance.HasNeighbour (Coordinates, Direction.RIGHT);
			
			if (wallAbove && wallBelow && !wallRight && !wallLeft) {
				_renderer.sprite = Wall_East;
			} else if (wallLeft && wallRight && !wallAbove && !wallBelow) {
				_renderer.sprite = Wall_North;
			} else if (wallAbove && wallRight && !wallLeft && !wallBelow) {
				_renderer.sprite = Wall_South_West;
			} else if (wallAbove && wallLeft && !wallRight && !wallBelow) {
				_renderer.sprite = Wall_South_East;
			} else if (wallBelow && wallRight && !wallLeft && !wallAbove) {
				_renderer.sprite = Wall_North_West;
			} else if (wallBelow && wallLeft && !wallRight && !wallAbove) {
				_renderer.sprite = Wall_North_East;
			} else if ((wallAbove || wallBelow) && (!wallRight && !wallLeft)) {
				_renderer.sprite = Wall_East;
			} else if ((wallRight || wallLeft) && (!wallAbove && !wallBelow)) {
				_renderer.sprite = Wall_North;
			}
		}

	}
}
