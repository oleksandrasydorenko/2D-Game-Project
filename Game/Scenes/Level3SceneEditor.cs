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
	public class Level3SceneEditor : TilemapEditor
	{
		public Level3SceneEditor(string name = "Level3Editor") : base(name) { }

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			// active tilemaps to draw
			tilemaps = new Tilemap[]
			{
                new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3Tilemap.json", rows: 50, columns: 60, name:"Foreground walkable"),
                new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3TilemapNoCollision.json", rows: 50, columns: 60, name: "Foreground not walkable"),
                new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level3/Level3TilemapBG.json", rows: 50, columns: 60),

                //new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3Tilemap.json", rows: 50, columns: 60, name:"Foreground walkable"),
				//new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level3TilemapNoCollision.json", rows: 50, columns: 60, name: "Foreground not walkable"),
                //new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level3TilemapBG.json", rows: 50, columns: 60, name: "Background"),
			};

            // where to save the level prefabs data to
            prefabFilePath = "Game/Assets/Prefabs/Level3/Level3ScenePrefabs.json";
            //prefabFilePath = "Level3ScenePrefabs.json";
		}

	}
}
