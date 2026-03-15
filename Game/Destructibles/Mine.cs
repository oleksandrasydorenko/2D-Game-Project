using JailBreaker.Effects;
using JailBreaker.Player;
using Raylib_cs;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Destructibles
{
    public class Mine : GameObject, IDestructable, IPrefable
    {
        
        public Vector2 BoundingBoxSize { get; set; } = new Vector2(8, 8);
        public int Health { get; set; } = 1;
        public int MaxHealth { get; set; } = 1;
        public Action onHealthZero { get; set; }
        public Action<GameObject> onDamageTaken { get; set; }
        Sprite mineSpriteSheet;
        SpriteComponent spriteComponent;
        private BoxCollider2D trigger;
        private BoxCollider2D collider;
        private bool isDestroyed = false;
        public override void Construct()
        {
            base.Construct();


            Name = "Mine";
            
            mineSpriteSheet = new Sprite("Game/Assets/Textures/Mine.png", 7);
            spriteComponent = new SpriteComponent(this, mineSpriteSheet, Color.White);
            SpriteAnimation mineAnimation = new SpriteAnimation(mineSpriteSheet, .25f, true);
            AnimationControllerState mineState = new AnimationControllerState("mine", mineAnimation);
            AnimationController animController = new AnimationController(mineState);
            SpriteAnimatorComponent animator = new SpriteAnimatorComponent(this, spriteComponent, animController);


            collider = new BoxCollider2D(this, 0, width: spriteComponent.sprite.FrameWidth * spriteComponent.SpriteScale * .75f, height: spriteComponent.sprite.FrameHeight * spriteComponent.SpriteScale * .75f);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.PassableObjects;
            
            trigger = new BoxCollider2D(this, 0, offsetY:-3, width: this.spriteComponent.sprite.FrameWidth * this.spriteComponent.SpriteScale * 1.75f, height: this.spriteComponent.sprite.FrameHeight * this.spriteComponent.SpriteScale * 1.75f);
            trigger.IsTrigger = true;
            //trigger.drawHitbox = true;
            trigger.alwaysCheckTriggers = true;

            //explode when player enters trigger
            trigger.onTriggerEntered += (other) =>
            {
                CollisionLayers target = other.CollisionLayer;
                if (target == CollisionLayers.Player) Destroy();
            };
            onHealthZero += Destroy;
        }
        /// <summary>
        /// destroys and add explosion effect
        /// </summary>
        public override void Destroy()
        {
            if (isDestroyed) return;
            isDestroyed = true;

            if(EngineSerivce.isEditor == false)
            {
				FloorExplosion exp = InstanceService.InstantiateWithPosition(new FloorExplosion(), new Vector2(GetPositionX(), GetPositionY() - 8));
			}

			InstanceService.Destroy(this);
            base.Destroy();

        }
    }
}
