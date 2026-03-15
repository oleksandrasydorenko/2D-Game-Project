using RocketEngine;

namespace JailBreaker.Scenes
{
    public class Level2Editor : TilemapEditor
    {
		public Level2Editor(string name = "Level2Editor") : base(name) { }

		public override void CreateTilemapEditor()
        {
            base.CreateTilemapEditor();

            // active tilemaps to draw
            tilemaps = new Tilemap[]
            {
                  new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2Tilemap.json", rows: 200, columns: 170, name:"Foreground walkable"),
                  new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2TilemapNoCollision.json", rows: 200, columns: 170, name: "Foreground not walkable"),
                  new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level2/Level2TilemapBG.json", rows: 200, columns: 170),
				//real Path
				  //new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2Tilemap.json", rows: 200, columns: 170, name:"Foreground walkable"),
                  //new Tilemap(new JailBreaker.PlatformsTileSheet(), "Game/Assets/Tilemaps/Level2TilemapNoCollision.json", rows: 200, columns: 170, name: "Foreground not walkable"),
                  //new Tilemap(new JailBreaker.WallsTileSheet(), "Game/Assets/Tilemaps/Level2TilemapBG.json", rows: 200, columns: 170, name: "Background"),
            };

            // where to save the level prefabs data to
            prefabFilePath = "Game/Assets/Prefabs/Level2/Level2Prefab.json";
            //prefabFilePath = "Level2Prefab.json";
        }

    }
}
