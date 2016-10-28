using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	[RequireComponent (typeof(SpriteRenderer))]
	public class SelectableTile : MonoBehaviour
	{
		public Vector2i Coordinates;
		private Sprite selectableSprite;
		public Sprite SelectableSprite { set { selectableSprite = value; } }

		private Sprite normalSprite;
		public Sprite NormalSprite { set { normalSprite = value; } }

		private SpriteRenderer _renderer;
		private bool selectable = true;
		public bool Selectable { 
			get { 
				return selectable; 
			} 
			set { 
				selectable = value;

				if (!selectable) {
					ShowNormal ();
				}
			}
		}


		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
		}

		void Start ()
		{
			normalSprite = _renderer.sprite;

			if (TileSelector.instance)
				TileSelector.instance.RegisterTile (this);
		}


		public void ShowSelectable ()
		{
			if (selectable) {
				_renderer.sprite = selectableSprite;
			}
		}

		public void ShowNormal ()
		{
			_renderer.sprite = normalSprite;
	
		}
	}
}
