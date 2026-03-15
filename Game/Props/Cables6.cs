using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Props
{
    public class Cables6 : Prop
    {
        public override Sprite Sprite { get; set; } = new Sprite("Game/Assets/MapDecoration/Cables6.png");
        public override Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);
    }
}
