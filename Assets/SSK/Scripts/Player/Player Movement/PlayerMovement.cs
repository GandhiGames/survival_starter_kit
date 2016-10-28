using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PlayerMovement : MonoBehaviour
	{
		public float MoveSpeed = 2.5f;

		void FixedUpdate ()
		{
			var move = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

			transform.Translate (move * MoveSpeed * Time.deltaTime, Space.World);

			var xBounds = FloorManager.instance.PlayerXBounds;
			var yBounds = FloorManager.instance.PlayerYBounds;

			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, xBounds [0].Value, xBounds [1].Value),
				Mathf.Clamp (transform.position.y, yBounds [0].Value, yBounds [1].Value),
				transform.position.z);
		}

	}
}
