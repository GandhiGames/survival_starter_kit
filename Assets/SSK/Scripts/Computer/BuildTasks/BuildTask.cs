using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public abstract class BuildTask : MonoBehaviour
	{
		public string friendlyName;
		public abstract bool Build ();
	}
}
