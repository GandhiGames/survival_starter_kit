using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(LineRenderer))]
	public class GunLaserSight : GunComponent
	{

		public float Range = 5f;
		public LayerMask Ignored;
	
		private LayerMask mask;
		private LineRenderer lineRenderer;

		public void Awake ()
		{
			mask = ~Ignored; // Anything but default mask.
		
			lineRenderer = GetComponent<LineRenderer> ();
		
			if (!lineRenderer) {
				Debug.LogError ("Please ensure laser script is attached to an object with a line renderer");
			}
		
			lineRenderer.SetPosition (1, new Vector3 (0, Range, 0));

			lineRenderer.enabled = false;
		}

		void Update ()
		{
			if (lineRenderer.enabled) {
				var hit = Physics2D.Raycast (transform.position, transform.up, Range, mask);
		
		
				if (hit.collider) {
					lineRenderer.SetPosition (1, new Vector3 (0, hit.distance, 0));
				} else {
					lineRenderer.SetPosition (1, new Vector3 (0, Range, 0));
				}
			}
		
		}

		public override void OnPickup ()
		{
			lineRenderer.enabled = true;
		}

		public override void OnDrop ()
		{
			lineRenderer.enabled = false;
		}
	}
}
