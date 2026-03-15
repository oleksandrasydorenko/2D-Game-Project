using JailBreaker.Data;
using Raylib_cs;
using RocketEngine.DataStore;
using RocketEngine.Tilemapsystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// BUG GRXI INDEX SET NOT FOUND IM SAVE

namespace RocketEngine
{
	public class Tilemap : GameObject, ISortingLayer
	{
		public GridComponent Grid { get; private set; }
		private int[,] gridCells;
		private Tile[,] placedTiles;

		public TileSheet TileSheet {  get; set; }

		private TilemapDataPreset tilemapData;

		public string FilePathToLoadTilemapFrom {  get; init; } // the path for the json file 

		#region Sorting
		private SortingLayers sortingLayer = SortingLayers.Background;
		public SortingLayers SortingLayer
		{
			get { return sortingLayer; }
			set 
			{ 
				Utils.SortingLayerHelper.ChangeSortingLayer(this, ref sortingLayer, value, false);
				UpdateSortingLayersOfAllTiles();
			}
		}

		private int zIndex = 0;
		public int ZIndex
		{
			get { return zIndex; }
			set 
			{ 
				Utils.SortingLayerHelper.ChangeZIndex(this, ref zIndex, value, false);
				UpdateSortingLayersOfAllTiles();
			}
		}
		#endregion

		private void UpdateSortingLayersOfAllTiles()
		{
			foreach (Tile tile in placedTiles)
			{
				if(tile != null)
				{
					tile.spriteComponent.SortingLayer = SortingLayer;
					tile.spriteComponent.ZIndex = ZIndex;
				}
			}
		}


		private int cellSize, columns, rows;
		private bool visible, hasCollision;
		public Tilemap(TileSheet tileSheet,string filePathToLoadTilemapFrom = "", int cellSize = 16, int rows = 100, int columns = 100, bool hasCollison = false,int x = 0, int y = 0, string name = "Tilemap"):base(x,y,name)
		{
			TileSheet = tileSheet;
			this.cellSize = cellSize;
			this.columns = columns;
			this.rows = rows;
			this.visible = visible;
			this.hasCollision = hasCollison;

			gridCells = new int[columns, rows];
			placedTiles = new Tile[columns, rows];

			FilePathToLoadTilemapFrom = filePathToLoadTilemapFrom;
		}

		public override void Construct()
		{
			base.Construct();

			Grid = new GridComponent(this, cellSize, rows, columns ,visible: false); // normaly the grid is not visible 

			if (FilePathToLoadTilemapFrom != "" && FilePathToLoadTilemapFrom != string.Empty) // wir versuchen eine tilemap zu laden 
			{
				bool success = LoadTilemap();

				if (success) // there is data in the tilemap that got loaded so we fill the grid cells with the data
				{
					Utils.ArrayUtils.Fill2DArrayWithJaggedArray(tilemapData.GridIndexSet, gridCells);
				}
			}

			//TintTilemap(Color.White);
		}

		/*
		public override void Destroy()
		{
			base.Destroy();


			ClearWholeTilemap();
		}
		*/
		public bool PlaceTileInGridCell(int tileID, Vector2 gridCell)
		{
		
			try
			{
				if (TileSheet.tileDictionary.ContainsKey(tileID)) // hier auch und auch des default tile sheet austauchen
				{
					Tile tileToPlace = InstanceService.Instantiate(new Tile(TileSheet.tileDictionary[tileID])); // hier auch 

					tileToPlace.SetPosition((gridCell * Grid.CellSize) + GetPosition());

					DeleteTileInGridCell(gridCell);

					gridCells[(int)gridCell.X, (int)gridCell.Y] = tileID; // change to the right index grid in the static dicitonary and then the index
					placedTiles[(int)gridCell.X, (int)gridCell.Y] = tileToPlace; // change to the right index grid in the static dicitonary and then the index

					tileToPlace.spriteComponent.SortingLayer = SortingLayer;
					tileToPlace.spriteComponent.ZIndex = ZIndex;

					if(hasCollision)
					{
						BoxCollider2D tileCollider = new BoxCollider2D(tileToPlace,tileToPlace.spriteComponent.TextureOffset.X, tileToPlace.spriteComponent.TextureOffset.X, cellSize, cellSize);
						tileCollider.IsCollider = true;
                        //tileCollider.drawHitbox = true;

					}

					return true;
				}
			}
			catch
			{
				return false;
			}
			

			return false;
			
		}

		public void FillWholeTilemap(int tileID)
		{
			for (int i = 0; i < gridCells.GetLength(0); i++)
			{
				for (int j = 0; j < gridCells.GetLength(1); j++)
				{
					PlaceTileInGridCell(tileID, new Vector2(i, j));
				}
			}
		}

		public void ClearWholeTilemap()
		{
			for (int i = 0; i < gridCells.GetLength(0); i++)
			{
				for (int j = 0; j < gridCells.GetLength(1); j++)
				{
					DeleteTileInGridCell(new Vector2(i, j));
				}
			}
		}

		private Color tilemapTint = Color.White;
		public void TintTilemap(Color tint)
		{
			tilemapTint = tint;

            Console.WriteLine("TINT");

			for (int i = 0; i < gridCells.GetLength(0); i++)
			{
				for (int j = 0; j < gridCells.GetLength(1); j++)
				{
					if (placedTiles[i, j] != null)
						placedTiles[i, j].spriteComponent.colorTint = Utils.ColorUtils.Multiply(placedTiles[i, j].spriteComponent.colorTint, tilemapTint);
				}
			}

		}

		public void RemoveTilemapTint()
		{
			for (int i = 0; i < gridCells.GetLength(0); i++)
			{
				for (int j = 0; j < gridCells.GetLength(1); j++)
				{
					if (placedTiles[i,j] != null)
						placedTiles[i, j].spriteComponent.colorTint = Utils.ColorUtils.Divide(placedTiles[i, j].spriteComponent.colorTint, tilemapTint);
				}
			}
			tilemapTint = Color.White;
			//TintTilemap(Color.White);
		}

		public bool DeleteTileInGridCell(Vector2 gridCell)
		{
			int cellX = (int)gridCell.X;
			int cellY = (int)gridCell.Y;

			if (placedTiles[cellX,cellY] != null)
			{
				InstanceService.Destroy(placedTiles[cellX, cellY]);

				placedTiles[cellX, cellY] = null;
				gridCells[cellX, cellY] = 0;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Tries to load fill the tilemap data variable with a saved tilemap data if success is false the tilemap data variables GridIndexSet is the same as the grid cells 
		/// </summary>
		/// <returns></returns>
		private bool LoadTilemap()
		{
			bool success = true;

			if (FilePathToLoadTilemapFrom == "" || FilePathToLoadTilemapFrom == string.Empty)
			{
				tilemapData = new TilemapDataPreset();

				

				// we dont want to load anything so we just convert the gird cells to a jagged array and give the grid index set that new array so its not a empty array 
				tilemapData.GridIndexSet = Utils.ArrayUtils.Convert2DArrayToJaggedArray(gridCells); 

				return success = false;
			}

			TilemapDataPreset? data = DataStore.DataStoreService.LoadFromJson<TilemapDataPreset>(FilePathToLoadTilemapFrom);

			if (data == null) // there is no data so we make it our self and fill it with default values 
			{
				data = new TilemapDataPreset();
				data.GridIndexSet = Utils.ArrayUtils.Convert2DArrayToJaggedArray(gridCells);

				// this creates a new json file to write to later
				bool dataSaved = DataStoreService.SaveToJson(FilePathToLoadTilemapFrom, data);

				tilemapData = data;

				success = false; // success false because the saved array is the same as the grid cells array
			}
			else // data found
			{
				tilemapData = data;

				Utils.ArrayUtils.Fill2DArrayWithJaggedArray(tilemapData.GridIndexSet, gridCells);

				// the grid cells are now filled with the right index so we can place them
				for(int i = 0; i < gridCells.GetLength(0); i++)
				{
					for(int j = 0; j < gridCells.GetLength(1); j++)
					{
						if (gridCells[i,j] > 0)
						{
							PlaceTileInGridCell(gridCells[i, j], new Vector2(i,j));
						}
					}
				}
			}

			return success;
		}

		/// <summary>
		/// Tries to convert the grid cells array to the tilemapDatas.GridIndexSet and then save it to json
		/// </summary>
		/// <returns></returns>
		public bool SaveTilemap()
		{
			if (FilePathToLoadTilemapFrom == "" || FilePathToLoadTilemapFrom == string.Empty) return false;

			// the grid Grid index set will get replaced by the grid cells array and then saved
			tilemapData.GridIndexSet = Utils.ArrayUtils.Convert2DArrayToJaggedArray(gridCells);

			return DataStoreService.SaveToJson<TilemapDataPreset>(FilePathToLoadTilemapFrom, tilemapData);
		}

		public void HideTilemap(bool hide)
		{
			foreach(Tile tile in placedTiles)
			{
				if (tile != null)
				{
					tile.spriteComponent.visible = !hide;
				}
			}

		}

	}
}
