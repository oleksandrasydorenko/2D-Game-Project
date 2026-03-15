using JailBreaker.Effects;
using RocketEngine;
using System.Numerics;

namespace JailBreaker.Game.Classes.Weapons.Projectiles
{
    public class Bomb : GameObject
    {
        public GameObject origin;
        Sprite bombSpriteSheet;
        SpriteComponent spriteComponent;
        private BoxCollider2D trigger;
        private PhysicsComponent physics;
        private bool isDestroyed = false;

        public override void Construct()
        {
            base.Construct();


            Name = "Bomb";
            bombSpriteSheet = new Sprite("Game/Assets/Textures/Bomb.png", 1);
            spriteComponent = new SpriteComponent(this, bombSpriteSheet, Raylib_cs.Color.White);

            PhysicsComponent physics = new PhysicsComponent(this, 5f, "PhysicsComponent");
            physics.Groundy0 = false;
            this.physics = physics;
            this.physics.Drag = 5f;


            trigger = new BoxCollider2D(this, 0, width: spriteComponent.sprite.FrameWidth * spriteComponent.SpriteScale * 1.75f, height: spriteComponent.sprite.FrameHeight * spriteComponent.SpriteScale * 1.75f);
            trigger.IsTrigger = true;
            //trigger.drawHitbox = true;
            trigger.onTriggerEntered += (other) =>
            {
                //mine should explode when colliding
                if (other.IsTrigger) return;
                CollisionLayers target = other.CollisionLayer;
                if (target == CollisionLayers.Enemy || target == CollisionLayers.Player || target == CollisionLayers.PassableObjects || target == CollisionLayers.GhostEnemy) return;

                Destroy();
            };
        }

        /// <summary>
        /// destroy and set explode animation
        /// </summary>
        public override void Destroy()
        {
            if (isDestroyed) return;
            isDestroyed = true;
            FloorExplosion exp = InstanceService.InstantiateWithPosition(new FloorExplosion(), new Vector2(GetPositionX(), GetPositionY() - 8));

            InstanceService.Destroy(this);
            base.Destroy();

        }
    }
}
