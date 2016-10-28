using UnityEngine;
using System.Collections;

namespace SurvivalKit
{
	public class TileList<T>
	{
		private T[,] tiles;
		private int width, height;

		public TileList (int width, int height)
		{
			this.width = width;
			this.height = height;

			tiles = new T[width, height];
		}

		public void InitialiseAll (T value)
		{
			for (int x = 0; x < width; x++) {
				for (int y = 0; y <height; y++) {
					tiles [x, y] = value;
				}
			}
		}

		public void Add (Vector2i coord, T obj)
		{
			if (InBounds (coord)) {
				tiles [coord.X, coord.Y] = obj;
			}
		}

		public T Get (Vector2i coord)
		{
			if (InBounds (coord)) {
				return tiles [coord.X, coord.Y];
			}

			return default (T);
		}

		public T GeRandom ()
		{
			return tiles [Random.Range (0, width), Random.Range (0, height)];
		}


		private bool InBounds (Vector2i coord)
		{
			return coord.X > 0 && coord.X < width && coord.Y > 0 && coord.Y < height;
		}
	}
}
