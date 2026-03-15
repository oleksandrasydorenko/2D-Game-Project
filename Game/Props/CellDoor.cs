using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Props
{
    public class CellDoor : Prop
    {
        public override Sprite Sprite { get; set; } = new Sprite("Game/Assets/MapDecoration/Cell.png");
        public override Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);

        SpriteComponent NumberRenderer1;
        SpriteComponent NumberRenderer2;

        public override void Construct()
        {
            base.Construct();

            Random rnd = new Random();
            
            Sprite Number1Sprite = new Sprite("Game/Assets/MapDecoration/NumbersAll.png", 10, rnd.Next(0,10));
            Sprite Number2Sprite = new Sprite("Game/Assets/MapDecoration/NumbersAll.png", 10, rnd.Next(0, 10));

            NumberRenderer1 = new SpriteComponent(this, Number1Sprite, Raylib_cs.Color.White, offsetX: -7, offsetY: -12);
            NumberRenderer2 = new SpriteComponent(this, Number2Sprite, Raylib_cs.Color.White, offsetX: -7, offsetY: -4);

        }
    }
}
