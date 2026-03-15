using JailBreaker.Destructibles;
using JailBreaker.Game.Effects;
using RocketEngine;
using RocketEngine.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class SniperBullet : ProjectileBase
    {
        private SpriteAnimatorComponent animator;
        public SniperBullet(float speed, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin) : base(speed, direction, position, damage, pierceAmount, origin)
        {
        }

        public override void Construct()
        {
            base.Construct();

            physics = new PhysicsComponent(this, 0, drag : 0.0f);
            sprite = new Sprite("Game/Assets/Textures/Weapons/BigLaser.png",7);
            Sprite deathSprite = new Sprite("Game/Assets/Textures/Weapons/SmallLaser.png",4);
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White, "SniperBullet");
            hitbox = new BoxCollider2D(this, 0, 0, 2, 1);
            hitbox.IsTrigger = true;
            hitbox.drawHitbox = false;
            hitbox.onTriggerEntered += HitObject;
            CalculateCollisionLayer();

            SpriteAnimation animation = new SpriteAnimation(sprite, .5f, false);
            AnimationControllerState flying = new AnimationControllerState("Flying", animation);
            SpriteAnimation deathAnimation = new SpriteAnimation(deathSprite, .5f, false);
            AnimationControllerState dying = new AnimationControllerState("Dying", deathAnimation);   
            deathAnimation.onAnimationFinished += () =>
            {
                InstanceService.Destroy(this);
            };
            AnimationControllerState[] allStates=
            {
                flying, 
                dying,
            };

            AnimationController controller = new AnimationController(allStates);
            animator = new SpriteAnimatorComponent(this, renderer, controller);
        }

        public override void Start()
        {
            base.Start();
            Console.WriteLine(direction);
            physics.AddForce(direction, speed, speed * 100);
            physics.Groundy0 = false;
            RotateTowards(direction);
            animator.SetState("Flying");
        }

        public override void Update()
        {
            
            base.Update();
            
        }
        
        public override void HitObject(BoxCollider2D obj)
        {
            // how to access ground property
			//GameObject o = obj.Parent as GameObject;
			//Console.WriteLine(o.GroundProperty.Ground);
          
			IDestructable destructable = obj.Parent as IDestructable;
            if(destructable != null && obj.Parent != origin && obj.IsCollider)
            {
                
                destructable.TakeDamage(damage, origin);
                pierceAmount--;
                if (pierceAmount <= 0)
                {
                    physics.Velocity = physics.Velocity / 10;
                    animator.PauseAnimator(false);
                    GameObject o = obj.Parent as GameObject;
                    switch (o.GroundProperty.Ground)
                    {
                        case GroundLayer.Default:
                            break;
                        case GroundLayer.Body:
                            InstanceService.Instantiate(new ImpactEffect("Blood", -GetForwardVector(), GetPosition()));
                            break;
                        case GroundLayer.Wood:
                            InstanceService.Instantiate(new ImpactEffect("Wood", -GetForwardVector(), GetPosition()));
                            break;
                        case GroundLayer.Metal:
                            InstanceService.Instantiate(new ImpactEffect("Metall", -GetForwardVector(), GetPosition()));
                            break;
                    }
                    animator.SetState("Dying");
                }
            }
            Tile tile = obj.Parent as Tile;
            if(tile != null)
            {
                physics.Velocity = physics.Velocity / 10;
                animator.PauseAnimator(false);
                GameObject o = obj.Parent as GameObject;
                switch (o.GroundProperty.Ground)
                {
                    case GroundLayer.Default:
                        break;
                    case GroundLayer.Body:
                        InstanceService.Instantiate(new ImpactEffect("Blood", -GetForwardVector(), GetPosition()));
                        break;
                    case GroundLayer.Wood:
                        InstanceService.Instantiate(new ImpactEffect("Wood", -GetForwardVector(), GetPosition()));
                        break;
                    case GroundLayer.Metal:
                        InstanceService.Instantiate(new ImpactEffect("Metall", -GetForwardVector(), GetPosition()));
                        break;
                }
                animator.SetState("Dying");
            }
        }

        
    }
}
