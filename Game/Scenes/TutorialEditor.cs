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
	public class TutorialEditor : TilemapEditor
	{
		public TutorialEditor(string name = "TutorialEditor") : base(name) { }

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			// active tilemaps to draw
			tilemaps = new Tilemap[]
			{
				// Local Path
				new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemap.json", rows: 50, columns: 120, name:"Foreground walkable"),
				new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemapNoCollion.json", rows: 50, columns: 120, name: "Foreground not walkable"),
				new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Tutorial/TutorialLevelTilemapBG.json", rows: 50, columns: 120, name: "Background"),

				// real Path
				//new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemap.json", rows: 50, columns: 120, name:"Foreground walkable"),
				//new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemapNoCollion.json", rows: 50, columns: 120, name: "Foreground not walkable"),
				//new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/TutorialLevelTilemapBG.json", rows: 50, columns: 120, name: "Background"),

			};

			// where to save the level prefabs data to
			prefabFilePath = "Game/Assets/Prefabs/Tutorial/TutorialPrefabs.json";
			//prefabFilePath = "TutorialPrefabs.json";
		}

	}
}
