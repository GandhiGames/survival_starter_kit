using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class PlayerLook : MonoBehaviour
	{
	
		void Update ()
		{
			if (Input.GetJoystickNames ().Length == 0) {
				var mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				var heading = mousePos - transform.position;
				var dist = heading.magnitude;
				var dir = heading / dist;
	
				if (dist > 5.02f) {
					var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
					var newRotation = Quaternion.AngleAxis (angle, Vector3.forward);

					if (Quaternion.Angle (newRotation, transform.rotation) > 3f)
						transform.rotation = newRotation;
				}
			} else {


				var dir = new Vector2 (Input.GetAxis ("Horizontal Right Stick"), Input.GetAxis ("Vertical Right Stick"));
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg; 

				var newRotation = Quaternion.AngleAxis (angle, Vector3.forward);

				if (dir.magnitude > 0.05f)
					transform.rotation = newRotation;
			}
		}
	}
}
