using JailBreaker.Player;
using RocketEngine;
using System.Numerics;

namespace JailBreaker.Game
{
    public class Platform : GameObject, IPrefable
    {
        public float Speed;
        public Vector2 Range = new Vector2();
        public int Direction = -1;
        SpriteComponent renderer;
        BoxCollider2D collider;
        BoxCollider2D trigger;
        List<GameObject> onPlattform = new List<GameObject>();
        int countCollider;
        Vector2 lastPosition;

        public Vector2 BoundingBoxSize { get; set; } = new Vector2(64, 16);

        public override void Construct()
        {
            base.Construct();
            Name = "Platform";

            Sprite sprite = new Sprite("Game/Assets/Textures/Plattform.png", 1);
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White);

            collider = new BoxCollider2D(this, width: 64, height: 16);
            collider.IsCollider = true;

            trigger = new BoxCollider2D(this,offsetY:-2, width: 67, height: 18);
            trigger.IsTrigger = true;
            trigger.onTriggerEntered += (BoxCollider2D obj) =>
            {
                if (obj.Parent as LaniasPlayer != null && obj.IsTrigger && obj.CollisionLayer == CollisionLayers.Player)
                {
                    if (!onPlattform.Contains(obj.Parent))
                        onPlattform.Add((GameObject)obj.Parent);
                }
                else if (obj.Parent as Enemy.Enemy != null && obj.IsCollider)
                    onPlattform.Add((GameObject)obj.Parent);
                else if (obj.IsCollider && obj.CollisionLayer == CollisionLayers.Default)
                {
                    countCollider++; ChangeDirection();
                }
            };
            trigger.onTriggerExited += (BoxCollider2D obj) =>
            {
                if (onPlattform.Contains(obj.Parent))
                    onPlattform.Remove((GameObject)obj.Parent);
                else if (obj.IsCollider && obj.CollisionLayer == CollisionLayers.Default)
                    countCollider--;
            };

            Speed = 35f;
            Range = new Vector2(GetPositionX() - 100, GetPositionX() + 200);
            renderer.ZIndex = 3;
        }
        public override void Update()
        {
            base.Update();
            if (GetPositionX() <= Range.X)
                Direction = 1;
            else if (GetPositionX() >= Range.Y)
                Direction = -1;
            lastPosition = GetPosition();
            SetPositionX(GetPositionX() + Speed * Direction * Time.DeltaTime);
            MoveObject();
        }
        private void MoveObject()
        {
            Vector2 platformDelta = GetPosition() - lastPosition;
            foreach (var obj in onPlattform)
                obj.SetPositionX(obj.GetPositionX() + platformDelta.X);
        }
        private void ChangeDirection()
        {
            if (countCollider > 1)
                return;
            Direction *= -1;
        }
    }
}
