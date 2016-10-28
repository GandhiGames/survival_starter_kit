using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class CrowAI : MonoBehaviour
	{
		public float attackDistance;

		private Transform _target;

		// Use this for initialization
		void Start ()
		{
			var player = GameObject.FindGameObjectWithTag ("Player");
			
			if (player) {
				_target = player.transform;
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void NoseDive ()
		{

		}
	}
}
