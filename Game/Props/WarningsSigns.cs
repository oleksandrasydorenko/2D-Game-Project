using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Props
{
    public class WarningSigns : Prop
    {
        public override Sprite Sprite { get; set; }
        public override Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);

        public override void Construct()
        {
            var rnd = new Random();
            Sprite = new Sprite("Game/Assets/MapDecoration/WarningSigns.png", 2, rnd.Next(0, 2));

            base.Construct();
        }
    }
}
