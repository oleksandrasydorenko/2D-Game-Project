using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RocketEngine 
{
    // i need to use a struct to copy the tiles to the tilemap 
	
    public struct TileArguments
	{
		public Sprite sprite;
		public Raylib_cs.Color tint = Raylib_cs.Color.White;

    
		public TileArguments(Sprite sprite, Raylib_cs.Color color)
		{
			this.sprite = sprite;
			this.tint = color;
		}
     
	}
    
	public class Tile : GameObject
    {
        public Sprite Sprite { get; init; }
        public SpriteComponent spriteComponent {  get; init; }
        public Raylib_cs.Color Tint { get; init; }

        public Tile(TileArguments arguments, float x =  0, float y = 0, string name = "Tile") : base(x,y,name)
        {
            this.Sprite = arguments.sprite;

			spriteComponent = new SpriteComponent(this, Sprite, arguments.tint);
            
            // the 0,0 point ot the sprite component is in the middle of the sprite but the one of the grid cell is in the top left corner
            spriteComponent.TextureOffset = new Vector2(spriteComponent.TextureOffset.X + Sprite.FrameWidth / 2, spriteComponent.TextureOffset.Y + Sprite.FrameHeight / 2);

			
		}
	}
}
