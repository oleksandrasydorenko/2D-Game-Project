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
	public class Level1Editor : TilemapEditor
	{
		public Level1Editor(string name = "Level1Editor") : base(name) { }

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			tilemaps = new Tilemap[]
			{
				new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1Tilemap.json", rows: 80, columns: 100, name:"Foreground walkable"),
				new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1TilemapNoCollision.json", rows: 80, columns: 100, name: "Foreground not walkable"),
				new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level1/Level1TilemapBG.json", rows: 80, columns: 100),
				//lokal
				//new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1Tilemap.json",rows: 65,columns: 100, name:"Foreground walkable"),
                //new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level1TilemapNoCollision.json",rows: 80,columns: 100, name: "Foreground not walkable"),
				//new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level1TilemapBG.json",rows: 65, columns: 100, name: "Background"),
			};

            prefabFilePath = "Game/Assets/Prefabs/Level1/Level1Prefabs.json";
            //prefabFilePath = "Level1Prefabs.json";

			//player = InstanceService.Instantiate(new LaniasPlayer());
			//player.SetPosition(0, 0);
			//player.walkForce = 0;
		}

	}
}
