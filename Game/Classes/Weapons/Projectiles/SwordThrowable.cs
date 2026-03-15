using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketEngine;
using System.Numerics;
using JailBreaker.Destructibles;
namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class SwordThrowable : ProjectileBase
    {
        private AnimationController controller;
        public SwordThrowable(float speed, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin) : base(speed, direction, position, damage, pierceAmount, origin)
        {
        }

        public override void Construct()
        {
            base.Construct();

            physics = new PhysicsComponent(this, 2, drag: 0.0f);
            sprite = new Sprite("Game/Assets/Textures/Weapons/PlasmaSword.png", 8);
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White, "ProjectileRenderer", 0, 0);
            Sprite deathSprite = new Sprite("Game/Assets/Textures/Weapons/WeaponVaporize.png", 4);
            SpriteAnimation deathAnimation = new SpriteAnimation(deathSprite, 0.5f, false);
            SpriteAnimation flyingAnimation = new SpriteAnimation(sprite, 0, false);
            AnimationControllerState death = new AnimationControllerState("Death", deathAnimation);
            AnimationControllerState flying = new AnimationControllerState("Flying", flyingAnimation);
            AnimationControllerState[] allStates =
            {
                death,
                flying
            };
            controller = new AnimationController(allStates);
            SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this, renderer, controller);
            controller.SetState("Flying");
            deathAnimation.onAnimationFinished += DestroyInstance;
            hitbox = new BoxCollider2D(this, 0, 0, 15, 15);
            hitbox.IsTrigger = true;
            hitbox.drawHitbox = false;
            hitbox.onTriggerEntered += HitObject;
            CalculateCollisionLayer();
            hitSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/BreakSound.mp3", false, use3DAudio: true, useDistanceBasedSound: true);
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
            base.Update();
            RotateTowards(physics.Velocity);
        }

        public override void HitObject(BoxCollider2D obj)
        {
            // how to access ground property
            //GameObject o = obj.Parent as GameObject;
            //Console.WriteLine(o.GroundProperty.Ground);

            IDestructable destructable = obj.Parent as IDestructable;
            if (destructable != null && obj.Parent != origin && obj.IsCollider)
            {

                destructable.TakeDamage(damage, origin);
                pierceAmount--;
                if (pierceAmount <= 0)
                {
                    physics.Velocity = Vector2.Zero;
                    controller.SetState("Death");
                    hitSound.Play();
                }
            }
            Tile tile = obj.Parent as Tile;
            if (tile != null)
            {
                physics.Velocity = Vector2.Zero;
                controller.SetState("Death");
                hitSound.Play();
            }
        }

        public void DestroyInstance()
        {
            InstanceService.Destroy(this);
        }


    }
}