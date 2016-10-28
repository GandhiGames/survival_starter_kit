using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalKit
{
	public enum Direction
	{
		ABOVE,
		BELOW,
		LEFT,
		RIGHT
	}
	;

	public class WallManager : MonoBehaviour
	{
		private TileList<Wall> wallList;

		private static WallManager _instance;
		public static WallManager instance { get { return _instance; } }

		private List<Wall> WallsInScene = new List<Wall> ();

		void Awake ()
		{
			_instance = this;
		}

		void Start ()
		{
			var width = (int)FloorManager.instance.RoomSize.width;
			var height = (int)FloorManager.instance.RoomSize.height;

			wallList = new TileList<Wall> (width, height);

		}

		public void Add (Wall wall, Vector2i coord)
		{
			wallList.Add (coord, wall);
			WallsInScene.Add (wall);
		}

		public Wall GetNeighbour (Vector2i coord, Direction direction)
		{
			if (direction == Direction.ABOVE) {
				return wallList.Get (new Vector2i (coord.X, coord.Y + 1));
			} else if (direction == Direction.BELOW) {
				return wallList.Get (new Vector2i (coord.X, coord.Y - 1));
			} else if (direction == Direction.LEFT) {
				return wallList.Get (new Vector2i (coord.X - 1, coord.Y));
			} else {
				return wallList.Get (new Vector2i (coord.X + 1, coord.Y));
			}
		}

		public bool HasNeighbour (Vector2i coord, Direction direction)
		{
			if (direction == Direction.ABOVE) {
				return wallList.Get (new Vector2i (coord.X, coord.Y + 1)) != default (Wall);
			} else if (direction == Direction.BELOW) {
				return wallList.Get (new Vector2i (coord.X, coord.Y - 1)) != default (Wall);
			} else if (direction == Direction.LEFT) {
				return wallList.Get (new Vector2i (coord.X - 1, coord.Y)) != default (Wall);
			} else {
				return wallList.Get (new Vector2i (coord.X + 1, coord.Y)) != default (Wall);
			}
		}

		public void OrientateAllWalls ()
		{
			foreach (var w in WallsInScene) {
				w.Orientate ();
			}
		}
	}
}
