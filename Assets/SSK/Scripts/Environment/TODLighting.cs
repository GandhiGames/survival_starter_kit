using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(Light))]
	public class TODLighting : MonoBehaviour
	{
		public float dayTimeInSeconds;
		public float nightTimeInSeconds;

		public float dayTimeToNightTransitionTime;
		public float nightTimeToDayTransitionTime;

		public float dayTimeLightIntensity;
		public float nightTimeLightIntensity;

		public bool isDayTime;

		private LerpOverTime _lerp;
		private Light _light;



		void Awake ()
		{
			_light = GetComponent<Light> ();

			if (isDayTime) {
				_light.intensity = dayTimeLightIntensity;
				StartCoroutine (StartNewPhase (dayTimeInSeconds, new LerpOverTime (dayTimeLightIntensity, nightTimeLightIntensity, dayTimeToNightTransitionTime)));
			} else {
				_light.intensity = nightTimeLightIntensity;
				StartCoroutine (StartNewPhase (nightTimeInSeconds, new LerpOverTime (nightTimeLightIntensity, dayTimeLightIntensity, nightTimeToDayTransitionTime)));
			}
		}

		// Update is called once per frame
		void Update ()
		{
			if (_lerp != null)
				_light.intensity = _lerp.Value;

			if (isDayTime && _light.intensity == nightTimeLightIntensity) {
				_lerp = null;
				StartCoroutine (StartNewPhase (nightTimeInSeconds, new LerpOverTime (nightTimeLightIntensity, dayTimeLightIntensity, nightTimeToDayTransitionTime)));
				isDayTime = false;
			} else if (!isDayTime && _light.intensity == dayTimeLightIntensity) {
				_lerp = null;
				isDayTime = true;
				StartCoroutine (StartNewPhase (dayTimeInSeconds, new LerpOverTime (dayTimeLightIntensity, nightTimeLightIntensity, dayTimeToNightTransitionTime)));
			}
		}

		private IEnumerator StartNewPhase (float seconds, LerpOverTime lerp)
		{
			yield return new WaitForSeconds (seconds);
			_lerp = lerp;
			_lerp.Start ();
		}
	}
}
