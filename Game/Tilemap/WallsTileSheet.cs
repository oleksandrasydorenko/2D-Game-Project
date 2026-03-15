using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker
{
	public class WallsTileSheet : TileSheet
	{

		const int AMOUNT_OF_TILES_ON_SHEET = 12;
		const string PATH = "Game/Assets/Textures/WallTiles.png";
		public WallsTileSheet()
		{
	
			tileDictionary = new Dictionary<int, TileArguments>
			{
				// hier drunter dann die weiteren indexe anlegen
				{1, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 0), Raylib_cs.Color.White)},
				{2, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 1), Raylib_cs.Color.White)},
                {3, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 2), Raylib_cs.Color.White)},
                {4, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 3), Raylib_cs.Color.White)},
                {5, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 5), Raylib_cs.Color.White)},
                {6, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 6), Raylib_cs.Color.White)},
                {8, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 7), Raylib_cs.Color.White)},
                {9, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 8), Raylib_cs.Color.White)},
                {10, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 9), Raylib_cs.Color.White)},
                {11, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 10), Raylib_cs.Color.White)},
                {12, new TileArguments(new Sprite(PATH,AMOUNT_OF_TILES_ON_SHEET, 11), Raylib_cs.Color.White)},

            };
		}
	

	}
}
