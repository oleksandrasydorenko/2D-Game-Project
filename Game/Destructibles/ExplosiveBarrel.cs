using JailBreaker.Game.Classes;
using JailBreaker.Interactibles;
using Raylib_cs;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static RocketEngine.Ui.UiElement;
using JailBreaker.Effects;
using RocketEngine.Physics;
using JailBreaker.Game.GroundLayers;

namespace JailBreaker.Destructibles
{



    public class ExplosiveBarrel : GameObject, IDestructable, IPrefable
    {
		
        private float timer = 0f;
        private float delay = 5f;
        private bool isDestroyed = false;
        Sprite barrelSpriteSheet;
        SpriteComponent spriteComponent;
        private int health;
        private BoxCollider2D collider;

        public override GroundProperty GroundProperty { get; set; } = new MetalGround();
        public Vector2 BoundingBoxSize { get; set; } = new Vector2(20, 20);

		public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
        private int maxHealth = 30;
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
            }
        }
        public Action onHealthZero { get; set; }
        public Action<GameObject> onDamageTaken { get; set; }


		public ExplosiveBarrel() : base() { }

        public override void Construct()
        {
            base.Construct();
            Name = "ExplosiveBarrel";

            Random rnd = new Random();
            int barrelRot = rnd.Next(0, 3);
            barrelSpriteSheet = new Sprite("Game/Assets/Textures/ExplosiveBarrel/ExplosiveBarrel.png", 5, barrelRot);
            spriteComponent = new SpriteComponent(this, barrelSpriteSheet, Raylib_cs.Color.White);
            

            onHealthZero += HealthZero;

            health = maxHealth;

            collider = new BoxCollider2D(this, 0, 5, width: spriteComponent.sprite.FrameWidth * spriteComponent.SpriteScale * .75f , height: spriteComponent.sprite.FrameHeight * spriteComponent.SpriteScale * .75f);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Obstacle;
            // collider.drawHitbox = true;
        }


        public override void Update()
        {
            base.Update();

            if (isDestroyed)
            {
                timer += Time.DeltaTime;
                if(timer >= delay)
                {
                    InstanceService.Destroy(this);
                }
            }
        }

        public void HealthZero()
        {
            if (isDestroyed) return;

            isDestroyed = true;
			
			collider.Destroy();

			Random rnd = new Random();
			int barrelRot = rnd.Next(3, 5);
            barrelSpriteSheet.CurrentFrame = barrelRot;

			Explosion exp = InstanceService.InstantiateWithPosition(new Explosion(), GetPosition());
		}
    }
}
