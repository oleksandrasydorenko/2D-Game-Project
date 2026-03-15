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
	public class JailBreakerTilemapEditor : TilemapEditor
	{
		LaniasPlayer player;

		public override void CreateTilemapEditor()
		{
			base.CreateTilemapEditor();

			// active tilemaps to draw
			tilemaps = new Tilemap[]
			{

				  new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel2.json",rows: 50,columns: 60),
				  new Tilemap(new JailBreaker.DefaultTileSheet(), "Game/Assets/Tilemaps/AlphaTilemapLevel2BG.json",rows: 50, columns: 60),
			};

			// where to save the level prefabs data to
			prefabFilePath = "prefabTest.json";
		}

	}
}
