using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using RocketEngine;

namespace JailBreaker.Props
{
    public abstract class Prop : GameObject, IPrefable
    {
        public virtual Sprite Sprite { get; set; } = new Sprite("Game/Assets/MapDecoration/Monitor1.png");
        public virtual Vector2 BoundingBoxSize { get; set; } = new Vector2(16,16);

        SpriteComponent Renderer;
        public override void Construct()
        {
            base.Construct();
            Name = "Prop";
            Renderer = new SpriteComponent(this, Sprite, Raylib_cs.Color.White);

        }
    }
}
