using JailBreaker.Destructibles;
using JailBreaker.Effects;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class PlasmaBall : ProjectileBase
    {
        public PlasmaBall(float speed, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin) : base(speed, direction, position, damage, pierceAmount, origin)
        {
        }

        public override void Construct()
        {
            base.Construct();

            physics = new PhysicsComponent(this, 3, drag: 0.0f);
            sprite = new Sprite("Game/Assets/Textures/Weapons/PlasmaBall.png",4);
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White, "ProjectileRenderer", 0, 0);
            renderer.SortingLayer = SortingLayers.Default;
            hitbox = new BoxCollider2D(this, 0, 0, 2, 1);
            hitbox.IsTrigger = true;
            hitbox.drawHitbox = false;
            hitbox.onTriggerEntered += HitObject;

            SpriteAnimation animation = new SpriteAnimation(sprite, .5f, true);
            AnimationControllerState flying = new AnimationControllerState("Flying", animation);
            AnimationController controller = new AnimationController(flying);
            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.SetState("Flying");
            
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine(direction);
            physics.AddForce(direction, speed, speed * 100);
            physics.Groundy0 = false;
            RotateTowards(direction);
            
        }

        public override void Update()
        {
            RotateTowards(Vector2.Normalize(physics.Velocity));
            base.Update();

        }

        public override void HitObject(BoxCollider2D obj)
        {
            // how to access ground property
            //GameObject o = obj.Parent as GameObject;
            //Console.WriteLine(o.GroundProperty.Ground);

            IDestructable destructable = obj.Parent as IDestructable;
            if (destructable != null && obj.Parent != origin && obj.IsCollider)
            {

                
                pierceAmount--;
                if (pierceAmount <= 0)
                {
                    Explosion exp = InstanceService.InstantiateWithPosition(new Explosion(), GetPosition());
                    InstanceService.Destroy(this);
                    
                }
            }
            Tile tile = obj.Parent as Tile;
            if (tile != null)
            {
                Explosion exp = InstanceService.InstantiateWithPosition(new Explosion(), GetPosition());
                InstanceService.Destroy(this);
                
            }
        }


    }
}
