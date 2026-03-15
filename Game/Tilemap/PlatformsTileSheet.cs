using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker
{
	public class PlatformsTileSheet : TileSheet
	{

		const int AMOUNT_OF_TILES_ON_SHEET = 47;
		const string PATH = "Game/Assets/Textures/PlattformTiles.png";
        public PlatformsTileSheet()
        {
            tileDictionary = new Dictionary<int, TileArguments>
            {
                { 1,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 0),  Raylib_cs.Color.White) },
                { 2,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 1),  Raylib_cs.Color.White) },
                { 3,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 2),  Raylib_cs.Color.White) },
                { 4,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 3),  Raylib_cs.Color.White) },
                { 5,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 4),  Raylib_cs.Color.White) },
                { 6,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 5),  Raylib_cs.Color.White) },
                { 7,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 6),  Raylib_cs.Color.White) },
                { 8,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 7),  Raylib_cs.Color.White) },
                { 9,  new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 8),  Raylib_cs.Color.White) },
                { 10, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 9),  Raylib_cs.Color.White) },
                { 11, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 10), Raylib_cs.Color.White) },
                { 12, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 11), Raylib_cs.Color.White) },
                { 13, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 12), Raylib_cs.Color.White) },
                { 14, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 13), Raylib_cs.Color.White) },
                { 15, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 14), Raylib_cs.Color.White) },
                { 16, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 15), Raylib_cs.Color.White) },
                { 17, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 16), Raylib_cs.Color.White) },
                { 18, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 17), Raylib_cs.Color.White) },
                { 19, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 18), Raylib_cs.Color.White) },
                { 20, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 19), Raylib_cs.Color.White) },
                { 21, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 20), Raylib_cs.Color.White) },
                { 22, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 21), Raylib_cs.Color.White) },
                { 23, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 22), Raylib_cs.Color.White) },
                { 24, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 23), Raylib_cs.Color.White) },
                { 25, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 24), Raylib_cs.Color.White) },
                { 26, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 25), Raylib_cs.Color.White) },
                { 27, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 26), Raylib_cs.Color.White) },
                { 28, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 27), Raylib_cs.Color.White) },
                { 29, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 28), Raylib_cs.Color.White) },
                { 30, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 29), Raylib_cs.Color.White) },
                { 31, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 30), Raylib_cs.Color.White) },
                { 32, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 31), Raylib_cs.Color.White) },
                { 33, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 32), Raylib_cs.Color.White) },
                { 34, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 33), Raylib_cs.Color.White) },
                { 35, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 34), Raylib_cs.Color.White) },
                { 36, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 35), Raylib_cs.Color.White) },
                { 37, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 36), Raylib_cs.Color.White) },
                { 38, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 37), Raylib_cs.Color.White) },
                { 39, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 38), Raylib_cs.Color.White) },
                { 40, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 39), Raylib_cs.Color.White) },
                { 41, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 40), Raylib_cs.Color.White) },
                { 42, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 41), Raylib_cs.Color.White) },
                { 43, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 42), Raylib_cs.Color.White) },
                { 44, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 43), Raylib_cs.Color.White) },
                { 45, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 44), Raylib_cs.Color.White) },
                { 46, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 45), Raylib_cs.Color.White) },
                { 47, new TileArguments(new Sprite(PATH, AMOUNT_OF_TILES_ON_SHEET, 46), Raylib_cs.Color.White) },
            };
        }


    }
}
