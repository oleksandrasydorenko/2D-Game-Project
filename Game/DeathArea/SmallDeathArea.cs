using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.DeathArea
{
    public class SmallDeathArea : DeathArea, IPrefable
    {
        public Vector2 BoundingBoxSize { get; set; } = new Vector2(64,16);
        public override void Construct()
        {
            base.Construct();
			Name = "SmallDeathArea";
			width = BoundingBoxSize.X;
            height = BoundingBoxSize.Y;
            trigger = new BoxCollider2D(this, 0,0, width, height);
            trigger.IsTrigger = true;
            trigger.AlwaysCheckTrigger = true;
            //trigger.drawHitbox = true;
            trigger.onTriggerEntered += (BoxCollider2D other) => CheckCollider(other);
        }
    }
}
