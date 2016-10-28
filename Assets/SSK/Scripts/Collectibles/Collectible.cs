using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Rigidbody2D))]
	public class Collectible : MonoBehaviour
	{

		public int Value;
		public float SeekDistance = 1f;
		public float SeperationDistance = 0.5f;
		public float SeperationWeight = 2f;
		public float SeekWeight = 1.5f;
		public LayerMask CollectibleLayer;
		public float MaxTimeAlive = 4f;

		private Rigidbody2D _rigidbody;
		private Transform player;
		private float currentTime;

		private bool _collected;

		void Awake ()
		{
			_rigidbody = GetComponent<Rigidbody2D> ();

		}

		// Use this for initialization
		void OnEnable ()
		{
			player = GameObject.FindGameObjectWithTag ("Player").transform;
			currentTime = 0f;
			_collected = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (_collected) {

				float step = 20f * Time.deltaTime;

				var targetPos = CashTotal.instance.screenPosition;

				transform.position = Vector2.MoveTowards (transform.position, targetPos, step);

				if ((targetPos - (Vector2)transform.position).sqrMagnitude < 0.05f) {
					CashTotal.instance.Increment (Value);
					ObjectManager.instance.RemoveObject (this.gameObject);
				}
			} else {
				NormalUpdate ();
			}
		}

		private void NormalUpdate ()
		{
			Vector2 force = Vector2.zero;
			
			currentTime += Time.deltaTime;
			
			if (currentTime < 1f) {
				force += GetSeperationForce () * SeperationWeight;
			} else if (currentTime >= MaxTimeAlive) {
				ObjectManager.instance.RemoveObject (this.gameObject);
			}
			
			force += GetSeekingForce () * SeekWeight;
			force += GetSeekingForce () * SeekWeight;
			
			_rigidbody.AddForce (force);
			
			var xBounds = FloorManager.instance.PlayerXBounds;
			var yBounds = FloorManager.instance.PlayerYBounds;
			
			transform.position = new Vector3 (
				Mathf.Clamp (transform.position.x, xBounds [0].Value, xBounds [1].Value),
				Mathf.Clamp (transform.position.y, yBounds [0].Value, yBounds [1].Value),
				transform.position.z);
		}

		private Vector2 GetSeperationForce ()
		{
			var others = Physics2D.OverlapCircleAll (transform.position, SeperationDistance, CollectibleLayer);
			
			Vector2 steeringForce = Vector2.zero;
			
			foreach (var o in others) {
				if (o.gameObject != this.gameObject) {
					Vector2 toAgent = transform.position - o.transform.position;
					steeringForce += toAgent.normalized / (toAgent.magnitude / 2f);
				}
			}

			return steeringForce;
		}

		private Vector2 GetSeekingForce ()
		{
			var heading = player.position - transform.position;
			var distance = heading.magnitude;

			var dir = Vector2.zero;

			if (distance < SeekDistance)
				dir = heading / distance;

			return dir;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Player")) {
				_collected = true;
			}
		}
	}
}
