using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class EnemyMenu : MonoBehaviour
	{
		public float speed;
		public Vector2 moveDirection;

		private bool _okToMove = false;

		private Vector2 _movedir;

		public void SpawnComplete ()
		{
			_okToMove = true;

			if (moveDirection != Vector2.zero) {
				_movedir = moveDirection;
			} else {

				var rand = Random.value;

	
				if (rand <= 0.25) {
					_movedir = Vector2.left;
				} else if (rand <= 0.5f) {
					_movedir = Vector3.right;
				} else if (rand <= 0.75f) {
					_movedir = Vector2.up;
				} else {
					_movedir = -Vector2.down;
				}
			}
		}
		// Update is called once per frame
		void Update ()
		{
			if (!_okToMove)
				return;



			var angle = Mathf.Atan2 (_movedir.y, _movedir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.Translate (_movedir * speed * Time.deltaTime, Space.World);
		}
	}
}
