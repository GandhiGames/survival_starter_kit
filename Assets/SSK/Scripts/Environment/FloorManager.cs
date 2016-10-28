using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SurvivalKit
{
	public class FloorManager : MonoBehaviour
	{
		public bool generateFloorOnStart = false;

		public GameObject FloorPrefab;
		public Sprite[] FloorSprites;
		public Sprite[] SelectableFloorSprites;
	
		public Rect RoomSize;

		private TileList<SelectableTile> cells;

		private readonly Vector2i enemySpawnArea = new Vector2i (3, 3);

		private float?[] playerXBounds = new float?[2];
		public float?[] PlayerXBounds { get { return playerXBounds; } }
		private float?[] playerYBounds = new float?[2];
		public float?[] PlayerYBounds { get { return playerYBounds; } }
		
		private static FloorManager _instance;
		public static FloorManager instance { get { return _instance; } }


		void Awake ()
		{
			_instance = this;

			cells = new TileList<SelectableTile> ((int)RoomSize.width, (int)RoomSize.height);

			if (FloorSprites.Length == 0 || SelectableFloorSprites.Length == 0) {
				Debug.LogError ("Floor sprites not set");
			}

			if (FloorSprites.Length != SelectableFloorSprites.Length) {
				Debug.LogError ("There should be a one-to-one mapping of floor and selectable floor sprites");
			}
		}

		void Start ()
		{
			if (generateFloorOnStart) {
				GenerateFloor ();
			}
		}

		public void GenerateFloor ()
		{
			var spawnManager = EnemySpawnManager.instance;

			var renderer = FloorPrefab.GetComponent<SpriteRenderer> ();
			float floorWidth = GetTileWidth (renderer.sprite);
			float floorHeight = GetTileHeight (renderer.sprite);
			
			for (int i = 0; i < RoomSize.width; i++) {
				var x = RoomSize.x + i * floorWidth;
				for (int j = 0; j < RoomSize.height; j++) {
					var y = RoomSize.y + j * floorHeight;
					
					var position = new Vector2 (x, y);
					var tileClone = (GameObject)Instantiate (FloorPrefab, position, Quaternion.identity);
					tileClone.transform.SetParent (transform);
					
					var index = GetSpriteIndex ();
					tileClone.GetComponent<SpriteRenderer> ().sprite = FloorSprites [index];
					
					var coord = new Vector2i (i, j);
					
					var tile = GetTile (tileClone);
					tile.Coordinates = coord;
					tile.NormalSprite = FloorSprites [index];
					tile.SelectableSprite = SelectableFloorSprites.Length > 0 ? SelectableFloorSprites [index] : null;

					SetBounds (tile);
					
					if (coord.X <= enemySpawnArea.X - 1 || coord.Y <= enemySpawnArea.Y - 1 || 
						coord.X >= (RoomSize.width - enemySpawnArea.X) || coord.Y >= (RoomSize.height - enemySpawnArea.Y)) {
						tile.Selectable = false;
						if (spawnManager)
							spawnManager.RegisterTile (tile);
					} else {
						cells.Add (coord, tile);
					}
					
					
				}
			}
		}

		private void SetBounds (SelectableTile tile)
		{
			if (tile.Coordinates.X == enemySpawnArea.X && !playerXBounds [0].HasValue) {
				playerXBounds [0] = tile.transform.position.x;
			} else if (tile.Coordinates.X == (RoomSize.width - enemySpawnArea.X - 1) && !playerXBounds [1].HasValue) {
				playerXBounds [1] = tile.transform.position.x;
			}

			if (tile.Coordinates.Y == enemySpawnArea.Y && !playerYBounds [0].HasValue) {
				playerYBounds [0] = tile.transform.position.y;
			} else if (tile.Coordinates.Y == (RoomSize.height - enemySpawnArea.Y - 1) && !playerYBounds [1].HasValue) {
				playerYBounds [1] = tile.transform.position.y;
			}
		}

		private int GetSpriteIndex ()
		{
			return Random.Range (0, FloorSprites.Length);
		}
	
		private SelectableTile GetTile (GameObject tileClone)
		{
			return tileClone.GetComponent<SelectableTile> ();
		}

		private float GetTileWidth (Sprite tile)
		{
			return tile.bounds.size.x;
		}
	
		private float GetTileHeight (Sprite tile)
		{
			return tile.bounds.size.y;
		}

		public SelectableTile GetRandomFreeFloorTile ()
		{
			SelectableTile tile = null;

			do {
				tile = cells.GeRandom ();
			} while (!tile.Selectable);

			return tile;
		}

		public List<SelectableTile> GetNeighbouringFreeTiles (Vector2i coord, Vector2i neighbourCount)
		{
			var retList = new List<SelectableTile> ();

			for (int x = coord.X - neighbourCount.X - 1; x < coord.X + neighbourCount.X; x++) {
				for (int y = coord.Y - neighbourCount.Y - 1; y < coord.Y + neighbourCount.Y; y++) {

					if (x == coord.X && y == coord.Y)
						continue;

					var tile = cells.Get (new Vector2i (x, y));

					if (tile && tile.Selectable) {
						retList.Add (tile);
					}
				}
			}

			return retList;

		}

		public List<SelectableTile> GetEncompassingFreeTiles (Vector2i centre, Vector2i widthHeight)
		{
			var retList = new List<SelectableTile> ();
			
			for (int x = centre.X - widthHeight.X; x < centre.X + widthHeight.X + 1; x++) {
				for (int y = centre.Y - widthHeight.Y; y < centre.Y + widthHeight.Y + 1; y++) {
					
					if (x == centre.X - widthHeight.X || x == (centre.X + widthHeight.X) 
						|| y == centre.Y - widthHeight.Y || y == (centre.Y + widthHeight.Y)) {

						
						var tile = cells.Get (new Vector2i (x, y));
						
						if (tile && tile.Selectable) {
							retList.Add (tile);
						}
					}
				}
			}
			
			return retList;
		}


		public List<SelectableTile> GetEncompassingFreeTilesWithEntries (Vector2i centre, Vector2i widthHeight)
		{
			var retList = new List<SelectableTile> ();
			
			for (int x = centre.X - widthHeight.X; x < centre.X + widthHeight.X + 1; x++) {
				for (int y = centre.Y - widthHeight.Y; y < centre.Y + widthHeight.Y + 1; y++) {
					
					if (x == centre.X - widthHeight.X || x == (centre.X + widthHeight.X) 
						|| y == centre.Y - widthHeight.Y || y == (centre.Y + widthHeight.Y)) {
		
						if ((x == centre.X - widthHeight.X && y == centre.Y) || (x == centre.X && y == centre.Y - widthHeight.Y)
							|| (x == centre.X + widthHeight.X && y == centre.Y) || (x == centre.X && y == centre.Y + widthHeight.Y))
							continue;

						if ((x == centre.X - widthHeight.X && y == centre.Y - 1) || (x == centre.X - 1 && y == centre.Y - widthHeight.Y)
							|| (x == centre.X + widthHeight.X && y == centre.Y - 1) || (x == centre.X - 1 && y == centre.Y + widthHeight.Y))
							continue;

						if ((x == centre.X - widthHeight.X && y == centre.Y + 1) || (x == centre.X + 1 && y == centre.Y - widthHeight.Y)
							|| (x == centre.X + widthHeight.X && y == centre.Y + 1) || (x == centre.X + 1 && y == centre.Y + widthHeight.Y))
							continue;

						var tile = cells.Get (new Vector2i (x, y));
					
						if (tile && tile.Selectable) {
							retList.Add (tile);
						}
					}
				}
			}

			return retList;
		}


		public SelectableTile GetFloorTile (Vector2i coord)
		{
			return cells.Get (coord);
		}

		public SelectableTile GetCentreTile ()
		{
			var centre = new Vector2i ((int)RoomSize.width / 2, (int)RoomSize.height / 2);
			return GetFloorTile (centre);
		}


		
	}
}
