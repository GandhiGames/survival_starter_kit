using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(GUITexture))]
	public class UIFlash : MonoBehaviour
	{

		private GameObject player;

		private bool isMenu, gameOver;

		private GUITexture _texture;

		void Start ()
		{
			player = GameObject.FindGameObjectWithTag ("Player");
			_texture = GetComponent<GUITexture> ();

			if (player == null) {
				isMenu = true;
			}

			var c = _texture.color;
			_texture.color = new Color (c.r, c.g, c.b, 0.5f);
		}

		// Update is called once per frame
		void Update ()
		{
			var c = _texture.color;

			if (isMenu != true) {
				if (_texture.color.a > 0 && gameOver == false) {
					_texture.color = new Color (c.r, c.g, c.b, c.a - Time.deltaTime / 2);
				}
				//if gameover is true, then we limit the alpha to 0.35 to give it a faded look when theres a game over
				if (_texture.color.a > 0.35 && gameOver == true) {
					_texture.color = new Color (c.r, c.g, c.b, c.a - Time.deltaTime / 2);
				}
				//if the alpha is less than or equal to 0 and the guitexture is enabled, we turn it off
				if (_texture.color.a <= 0 && _texture.enabled == true && gameOver == false) {
					_texture.enabled = false;
				}
				//if the alpha is greater than 0 and the guitexture is disabled, we turn it back on.
				if (_texture.color.a > 0 && _texture.enabled == false && gameOver == false) {
					_texture.enabled = true;
				}
			} else {
				//if it is the menu, we just do this and thats it.
				if (_texture.color.a > 0.2f) {
					_texture.color = new Color (c.r, c.g, c.b, c.a - Time.deltaTime / 2);
				}
			}
		}

		public void GameOverUIFlash ()
		{
			var c = _texture.color;
			_texture.color = new Color (c.r, c.g, c.b, 0.5f);

			_texture.enabled = true;

			gameOver = true;

		}
	}
}