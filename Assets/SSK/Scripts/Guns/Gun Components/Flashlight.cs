using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Light))]
	public class Flashlight : GunComponent
	{
		private Light _light;
		private bool equipped = false;

		// Use this for initialization
		void Awake ()
		{
			_light = GetComponent<Light> ();
			_light.enabled = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (equipped && Input.GetButtonDown ("Flashlight")) {
				_light.enabled = !_light.enabled;
			}
		}
		
		public override void OnPickup ()
		{
			_light.enabled = false;
			equipped = true;
		}

		public override void OnDrop ()
		{
			_light.enabled = false;
			equipped = false;
		}
	}
}
