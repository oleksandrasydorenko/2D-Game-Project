using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketEngine;
using System.Numerics;
using JailBreaker.Destructibles;
using JailBreaker.Player;
namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class ProjectileBase : GameObject
    {
        protected PhysicsComponent physics;
        public SpriteAnimatorComponent animator;
        protected Sprite sprite;
        protected SpriteComponent renderer;
        protected BoxCollider2D hitbox;
        protected Vector2 direction;
        protected float speed, x, y;
        float aliveTimer = 10.0f;
        protected int damage = 1;
        protected int pierceAmount = 1;
        protected GameObject origin;
        protected AudioComponent hitSound;
        public ProjectileBase(float speed, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin) : this(speed, new Sprite("Game/Assets/Textures/SimpleBullets-Sheet.png"), direction, position, damage, pierceAmount, origin)
        {
        }
        public ProjectileBase(float speed, Sprite sprite, Vector2 direction, Vector2 position, int damage, int pierceAmount, GameObject origin)
        {
            this.speed = speed;
            this.sprite = sprite;
            this.direction = direction;
            this.damage = damage;
            this.pierceAmount = pierceAmount;
            this.origin = origin;
            SetPosition(position);
        }


        public override void Update()
        {
            CheckAliveTime();
            base.Update();

        }

        public virtual void HitObject(BoxCollider2D obj)
        {
            
        }

        public void CheckAliveTime()
        {
            aliveTimer -= Time.DeltaTime;
            if (aliveTimer <= 0)
            {
                InstanceService.Destroy(this);
                Console.WriteLine("Bullet destroyed");
            }
        }
        public void CalculateCollisionLayer()
        {
            if (origin as Enemy.Enemy != null) hitbox.CollisionLayer = CollisionLayers.EnemyProjectile;
            else if (origin as LaniasPlayer != null) hitbox.CollisionLayer = CollisionLayers.PlayerProjectile;
            else hitbox.CollisionLayer= CollisionLayers.EnvironmentalProjectile;
        }
    }
}
