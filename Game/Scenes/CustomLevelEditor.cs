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
	public class CustomLevelEditor : TilemapEditor
	{
		public CustomLevelEditor(string name = "CustomLevelEditor") : base(name) { }

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			// active tilemaps to draw
			tilemaps = new Tilemap[]
			{

				new Tilemap(new JailBreaker.PlatformsTileSheet(), "CustomTilemap.json", rows: 50, columns: 120),
				new Tilemap(new JailBreaker.PlatformsTileSheet(), "CustomTilemapNoCollion.json", rows: 50, columns: 100),                
				new Tilemap(new JailBreaker.WallsTileSheet(), "CustomTilemapBG.json", rows: 50, columns: 100),
			};

			// where to save the level prefabs data to
			prefabFilePath = "CustomPrefabs.json";
		}

	}
}
