using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker
{
	public class DefaultTileSheet: TileSheet
	{

		private int tileSheetSize = 14;
		public DefaultTileSheet()
		{
	
			tileDictionary = new Dictionary<int, TileArguments>
			{
				{1, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 0), Raylib_cs.Color.White)},
				{2, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 1), Raylib_cs.Color.White)},
				{3, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 2), Raylib_cs.Color.White)},
				{4, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 3), Raylib_cs.Color.White)},
				{5, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 4), Raylib_cs.Color.White)},
				{6, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 5), Raylib_cs.Color.White)},
				{7, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 6), Raylib_cs.Color.White)},
				{8, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 7), Raylib_cs.Color.White)},
				{9, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 8), Raylib_cs.Color.White)},
				{10, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 9), Raylib_cs.Color.White)},
				{11, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 10), Raylib_cs.Color.White)},
				{12, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 11), Raylib_cs.Color.White)},
				{13, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 12), Raylib_cs.Color.White)},
				{14, new TileArguments(new Sprite("Game/Assets/Textures/Tilemap_Test.png",tileSheetSize, 13), Raylib_cs.Color.White)},
			};
		}
	

	}
}
