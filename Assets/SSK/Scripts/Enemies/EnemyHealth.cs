using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(AudioSource))]
	public class EnemyHealth : MonoBehaviour
	{
		public int MaxHealth = 10;
		private float currentHealth;

		public AudioClip[] OnHitSounds;
		public GameObject OnDeadAnimation;
		public GameObject[] OnDeadSprites;
		public GameObject[] CollectiblesPrefabs;
		public int MinCollectiblesDropped;
		public int MaxCollectiblesDropped;


		private float? dpsAmount = null;
	
		private AudioSource _audio;
	
		void Awake ()
		{
			if (OnDeadSprites == null || OnDeadSprites.Length == 0) {
				Debug.LogError ("Please set sprites to be shown when zombie dies");
			}
		
			_audio = GetComponent<AudioSource> ();
		
		}

		void OnEnable ()
		{
			currentHealth = MaxHealth;
		}

		private void PlayHitSound ()
		{
			if (OnHitSounds != null && OnHitSounds.Length > 0 && _audio)
				_audio.PlayOneShot (OnHitSounds [Random.Range (0, OnHitSounds.Length)]);
		}
	
		public void ApplyDamage (int damageAmount)
		{
			PlayHitSound ();
			currentHealth -= damageAmount;

			if (currentHealth <= 0f) {
				OnDead ();
			}
		}

		public void ApplyDPS (float dps, float time)
		{
			PlayHitSound ();
			this.dpsAmount = dps;
			Invoke ("DisableDPS", time);
		}

		private void DisableDPS ()
		{
			dpsAmount = null;
		}

		void Update ()
		{
			if (dpsAmount.HasValue) {
	
				currentHealth -= dpsAmount.Value * Time.deltaTime;

				if (currentHealth <= 0f) {
					OnDead ();
				}
			}
		}

		public void OnDead ()
		{
			if (OnDeadAnimation) {
				Instantiate (OnDeadAnimation, transform.position, Quaternion.identity);
			}

			if (CollectiblesPrefabs != null && CollectiblesPrefabs.Length > 0) {
				var numOfCollectibles = Random.Range (MinCollectiblesDropped, MaxCollectiblesDropped + 1);

				for (int i = 0; i < numOfCollectibles; i++) {
					var pos = new Vector2 (transform.position.x + Random.Range (-0.5f, 0.5f), transform.position.y + Random.Range (-0.5f, 0.5f));

					if (ObjectManager.instance)
						ObjectManager.instance.AddObject (CollectiblesPrefabs [Random.Range (0, CollectiblesPrefabs.Length)].name, pos);
				}
			}

			if (RoundDirector.instance)
				RoundDirector.instance.currentRound.EnemyKilled ();

			Instantiate (OnDeadSprites [Random.Range (0, OnDeadSprites.Length)], transform.position, Quaternion.identity);
		
			if (ObjectManager.instance) {
				ObjectManager.instance.RemoveObject (gameObject);
			} else {
				Destroy (gameObject);
			}
		}
	}
}

