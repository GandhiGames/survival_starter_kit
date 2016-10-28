using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class SimpleCollectible : MonoBehaviour
	{
		public Computer owner { private get; set; }

		// Update is called once per frame
		void Update ()
		{
			if (owner != null) {
				float step = 20f * Time.deltaTime;


				transform.position = Vector2.MoveTowards (transform.position, owner.transform.position, step);
			
				if (((Vector2)owner.transform.position - (Vector2)transform.position).sqrMagnitude < 0.02f) {
					owner.ShowMoneyText ();
					ObjectManager.instance.RemoveObject (this.gameObject);
				}
			}
		}
	}
}
