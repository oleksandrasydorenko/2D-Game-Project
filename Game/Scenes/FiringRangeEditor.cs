using JailBreaker.Player;
using JailBreaker.Scenes;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Scenes
{
	public class FiringRangeEditor : TilemapEditor
	{

		public FiringRangeEditor(string name = "FiringRangeEditor") : base(name) { }

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			// active tilemaps to draw
			tilemaps = new Tilemap[]
			{
				// real Path
				// new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/FiringRange/FiringRangeTilemap.json", rows: 50, columns: 120),
				// new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/FiringRange/FiringRangeTilemapNoCollion.json", rows: 50, columns: 120),
				// new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/FiringRange/FiringRangeTilemapBG.json", rows: 50, columns: 120),

				// local Path
				   new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/FiringRangeTilemap.json", rows: 50, columns: 120),
				   new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/FiringRangeTilemapNoCollion.json", rows: 50, columns: 120),
				   new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/FiringRangeTilemapBG.json", rows: 50, columns: 120),

			};

			// where to save the level prefabs data to
			//prefabFilePath = "Game/Assets/Prefabs/Tutorial/FiringRangePrefabs.json";
			prefabFilePath = "FiringRangePrefabs.json";
		}

	}
}
