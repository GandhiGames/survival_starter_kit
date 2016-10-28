using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	[RequireComponent (typeof(AudioSource))]
	public class GunBarrel : GunComponent
	{
		public float LaunchSpeed;
		public float VolumeScale = 0.2f;
		public AudioClip[] FireSounds;
		private bool audioEnabled = true;

		// If more than one muzzle flash found then random flash will be activated on fire.
		private List<Transform> muzzleFlashes;
		private GunClip gunClip;
	
		private bool knockBackApplied = false;
		private float? knockBackForce;


		void Start ()
		{
			muzzleFlashes = new List<Transform> ();

			Transform clip = null;
			foreach (Transform sibling in transform.parent) {
				if (sibling.CompareTag ("GunMuzzle")) {
					muzzleFlashes.Add (sibling);
				} else if (sibling.CompareTag ("GunClip")) {
					clip = sibling;
				} else if (sibling.CompareTag ("GunStock")) {
					knockBackApplied = true;
				
					var gunStock = sibling.GetComponent<GunStock> ();
				
					if (!gunStock) {
						Debug.LogError ("Object with GunStock tag should have GunStock script attached");
					} else {
						knockBackForce = gunStock.KnockBackForce;
					}
				}
			}

			if (!clip) {
				Debug.LogError ("Weapon requires a gun clip object with tag 'GunClip'");
			} else {
				gunClip = clip.GetComponent<GunClip> ();

				if (!gunClip) {
					Debug.LogError ("Gun clip object should have GunClip script attached");
				}
			}
		
			if (FireSounds == null || FireSounds.Length == 0) {
				Debug.Log ("No shoot sound found, audio on weapon shooting is disabled for this weapon");
				audioEnabled = false;
			}

		}

		private void SetMuzzleFlashActive (bool active)
		{
			if (IsMuzzleFlashActive ())
				return;

			if (muzzleFlashes.Count > 0) {
				muzzleFlashes [Random.Range (0, muzzleFlashes.Count)].gameObject.SetActive (active);
			}
		}

		private bool IsMuzzleFlashActive ()
		{
			foreach (var muzzleFlash in muzzleFlashes) {
				if (muzzleFlash.gameObject.activeSelf)
					return true;
			}

			return false;
		}

		private void DisableMuzzleFlashes ()
		{
			foreach (var muzzleFlash in muzzleFlashes) {
				muzzleFlash.gameObject.SetActive (false);
			}
		}

		public virtual void OnFire ()
		{
			if (gunClip == null)
				return;

			var bulletClone = gunClip.RequestBullet ();

			if (bulletClone != null) {
				Fire (bulletClone);
			}

		}

		private void Fire (GameObject bullet)
		{
			bullet.SetActive (true);
			bullet.transform.position = transform.position;
			bullet.transform.rotation = transform.rotation; 
		
			InitSmokeTrail (bullet);
		
			SetMuzzleFlashActive (true);
		
		
			bullet.GetComponent<Rigidbody2D> ().AddForce (bullet.transform.up * LaunchSpeed);
		
			PlayShootSound ();
		
			ApplyKnockBackForce (-bullet.transform.up);
		
		}
	
		private void InitSmokeTrail (GameObject bullet)
		{
			var smokeTrail = bullet.GetComponentInChildren<MissileTrail> ();
		
			if (smokeTrail) {
				smokeTrail.OnFire ();
			}
		}
	
		private void PlayShootSound ()
		{
			if (audioEnabled) {
				GetComponent<AudioSource> ().PlayOneShot (FireSounds [Random.Range (0, FireSounds.Length)], VolumeScale);
			}
		}
	
		private void ApplyKnockBackForce (Vector2 dir)
		{
			if (knockBackApplied && knockBackForce.HasValue) {
				transform.parent.parent.parent.GetComponent<Rigidbody2D> ().AddForce (dir * knockBackForce.Value);
			}
		}

		public override void OnPickup ()
		{
		}

		public override void OnDrop ()
		{
			DisableMuzzleFlashes ();
		}
	}
}
