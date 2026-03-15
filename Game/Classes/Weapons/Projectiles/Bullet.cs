using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketEngine;
using System.Numerics;
using JailBreaker.Destructibles;
using JailBreaker.Game.Effects;
using RocketEngine.Physics;
namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class Bullet : ProjectileBase
    {
        private SpriteAnimatorComponent animator;
        public Bullet(float speed, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin) : base(speed, direction, position, damage, pierceAmount, origin)
        {
        }

        public override void Construct()
        {
            base.Construct();

            physics = new PhysicsComponent(this, 0, drag : 0.0f);
            sprite = new Sprite("Game/Assets/Textures/Weapons/SmallLaser.png",4);
            renderer = new SpriteComponent(this, sprite, Raylib_cs.Color.White, "Bullet");
            hitbox = new BoxCollider2D(this, 0, 0, 2, 1);
            hitbox.IsTrigger = true;
            hitbox.drawHitbox = false;
            hitbox.onTriggerEntered += HitObject;
            CalculateCollisionLayer();

            SpriteAnimation animation = new SpriteAnimation(sprite, .5f, false);
            AnimationControllerState dying = new AnimationControllerState("Dying", animation);
            AnimationController controller = new AnimationController(dying);
            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();
            animation.onAnimationFinished += () =>
            {
                InstanceService.Destroy(this);
            };

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
            
        }
        
        public override void HitObject(BoxCollider2D obj)
        {
            // how to access ground property
			//GameObject o = obj.Parent as GameObject;
			//Console.WriteLine(o.GroundProperty.Ground);
          
			IDestructable destructable = obj.Parent as IDestructable;
            if(destructable != null && obj.Parent != origin && obj.IsCollider)
            {
                if (pierceAmount <= 0) return;
                destructable.TakeDamage(damage, origin);
                pierceAmount--;
                if (pierceAmount <= 0)
                {
                    physics.Velocity = physics.Velocity / 20;
                    animator.PauseAnimator(false);
                    GameObject o = obj.Parent as GameObject;
                    switch (o.GroundProperty.Ground) {
                        case GroundLayer.Default:
                            InstanceService.Instantiate(new ImpactEffect("Default", -GetForwardVector(), GetPosition()));
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
                physics.Velocity = physics.Velocity / 20;
                animator.PauseAnimator(false);
                GameObject o = obj.Parent as GameObject;
                switch (o.GroundProperty.Ground)
                {
                    case GroundLayer.Default:
                        InstanceService.Instantiate(new ImpactEffect("Default", -GetForwardVector(), GetPosition()));
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
