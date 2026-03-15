using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Props
{
    public class GunShots : Prop
    {
        public override Sprite Sprite { get; set; }
        public override Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);

        public GunShots() : base() { }
        public override void Construct()
        {
            Random rnd = new Random();
            Sprite = new Sprite("Game/Assets/MapDecoration/GunShots3.png", 3, rnd.Next(0, 3));

            base.Construct();
        }
    }
}
