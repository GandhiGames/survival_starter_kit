using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public abstract class GunComponent : MonoBehaviour
	{

		public abstract void OnPickup ();
		public abstract void OnDrop ();
	}
}
