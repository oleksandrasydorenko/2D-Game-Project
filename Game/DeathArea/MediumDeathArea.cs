using RocketEngine;
using System.Numerics;

namespace JailBreaker.DeathArea
{
    public class MediumDeathArea : DeathArea, IPrefable
    {
        public Vector2 BoundingBoxSize { get; set; } = new Vector2(256, 16);
        public override void Construct()
        {
            base.Construct();
			Name = "MediumDeathArea";
			width = BoundingBoxSize.X;
            height = BoundingBoxSize.Y;
            trigger = new BoxCollider2D(this, 0, 0, width, height);
            trigger.IsTrigger = true;
            trigger.AlwaysCheckTrigger = true;
            //trigger.drawHitbox = true;
            trigger.onTriggerEntered += (BoxCollider2D other) => CheckCollider(other);
        }

    }
}
