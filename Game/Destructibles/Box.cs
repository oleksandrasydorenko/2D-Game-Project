using JailBreaker.Enemy;
using JailBreaker.Game.Classes.Weapons;
using JailBreaker.Game.Interactibles.WeaponPickUps;
using JailBreaker.Game.GroundLayers;
using JailBreaker.Interactibles;
using Raylib_cs;
using RocketEngine;
using RocketEngine.Physics;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static JailBreaker.Enemy.Enemy;
using static RocketEngine.Ui.UiElement;

namespace JailBreaker.Destructibles
{

    /// <summary>
    /// destructible box that can be destroyed to spawn loot and enemies
    /// </summary>
    public class Box : GameObject, IDestructable, IPrefable
    {
        /// <summary>
        /// defines possible loot types
        /// </summary>
		public enum ChestLootOptions { Nothing, HealthPack, Key, Pistol, Sniper, MachineGun, ShotGun, PlasmaSword, PlasmaLauncher, Random}

        public override GroundProperty GroundProperty { get; set; } = new WoodGround();
		public ChestLootOptions chestLoot = ChestLootOptions.Nothing;

        public ChestLootOptions[] randomWeapons = {ChestLootOptions.Pistol, ChestLootOptions.Sniper ,ChestLootOptions.MachineGun, ChestLootOptions.ShotGun, ChestLootOptions.PlasmaLauncher, ChestLootOptions.PlasmaSword};

        private float timer = 0f;
        private float delay = 5f;
        private bool isDestroyed = false;
        protected SpriteComponent spriteComponent;
        private int health;
        private BoxCollider2D collider;
        int boxSprite;

		PhysicsComponent physics;
		public Vector2 BoundingBoxSize { get; set; } = new Vector2(16, 16);
		Random random = new Random();
		private int frameCount;
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
        private int maxHealth = 20;
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

        AudioComponent breakingSound;
        private string[] breakingSounds = { "Game/Assets/Audio/Destructibles/BreakingSound.wav", "Game/Assets/Audio/Destructibles/BreakingSound2.wav" };

		public override void Construct()
        {
            base.Construct();
			Name = "Box";
			boxSprite = random.Next(0, 3);
            boxSprite *= 3;
			
            spriteComponent = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Kiste2.png", 9, boxSprite), Raylib_cs.Color.White);
            spriteComponent.SpriteScale = 1f;
            spriteComponent.ZIndex = 5;

            //spriteComponent.SpriteScale = 0.5f;
            //spriteComponent.TransformChanged();
            onDamageTaken += DamageTaken;
            onHealthZero += HealthZero;

            health = maxHealth;

			physics = new PhysicsComponent(this, 2);
            physics.Groundy0 = false;
            physics.GravityMultiplier = 0;

			collider = new BoxCollider2D(this, width: spriteComponent.sprite.FrameWidth * spriteComponent.SpriteScale, height: spriteComponent.sprite.FrameHeight * spriteComponent.SpriteScale);
            collider.IsCollider = true;
            collider.CollisionLayer = CollisionLayers.Obstacle;
			//collider.drawHitbox = true;

			int randomSoundIndex = random.Next(0, breakingSounds.Length);
			breakingSound = new AudioComponent(this, breakingSounds[0], false, 0.35f, 1, true, true, 10, 250);

		}

		/// <summary>
		/// changes box appearence based on its health level
		/// </summary>
		/// <param name="orgin"></param>
		public void DamageTaken(GameObject orgin)
        {
            Console.WriteLine("Damage taken");

            switch (health)
            {
                case int h when h > maxHealth / 2:
                    spriteComponent.sprite.CurrentFrame = boxSprite + 0;
                    break;

                case int h when h <= maxHealth / 2 && h > maxHealth / 4:
                    spriteComponent.sprite.CurrentFrame = boxSprite + 1;
                    break;

                case int h when h <= 0:
                    spriteComponent.sprite.CurrentFrame = boxSprite + 2;
					break;
            }
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

            if(physics != null && physics.GravityMultiplier == 0)
            {
                physics.GravityMultiplier = 11;
            }
           
        }

        /// <summary>
        /// spawns loot based on its ChestLootOption
        /// </summary>
        public void SpawnLoot()
        {
            if (chestLoot != ChestLootOptions.Nothing)
            {
				Vector2 spawnPosition = GetPosition() + GetUpVector() * 10;

                ChestLootOptions lootToDrop;

                

				switch (chestLoot)
                {
                    case ChestLootOptions.HealthPack:
						HealthPack healthPack = InstanceService.InstantiateWithPosition(new HealthPack(), spawnPosition);
						break;

					case ChestLootOptions.Key:
						Key key = InstanceService.InstantiateWithPosition(new Key(), spawnPosition);
						break;

                    case ChestLootOptions.MachineGun:
                        MachineGunPickUp mg = InstanceService.InstantiateWithPosition(new MachineGunPickUp(), spawnPosition);
                        break;

					case ChestLootOptions.Pistol:
                        PistolPickUp pistol = InstanceService.InstantiateWithPosition(new PistolPickUp(), spawnPosition);
						break;

					case ChestLootOptions.Sniper:
						SniperPickUp sniper = InstanceService.InstantiateWithPosition(new SniperPickUp(), spawnPosition);
						break;

					case ChestLootOptions.ShotGun:
						ShotGunPickUp shotGun = InstanceService.InstantiateWithPosition(new ShotGunPickUp(), spawnPosition);
						break;

					case ChestLootOptions.PlasmaLauncher:
						PlasmaLauncherPickUp plasmaLauncher = InstanceService.InstantiateWithPosition(new PlasmaLauncherPickUp(), spawnPosition);
						break;

					case ChestLootOptions.Random:
						int percent = random.Next(0, 101);

                        switch (percent)
                        {
                            case <= 70:
                                chestLoot = ChestLootOptions.Nothing;
                                break;
                            case <= 85:
                                chestLoot = ChestLootOptions.HealthPack;
                                break;
                            case <= 100:
                                int weaponIndex = random.Next(0, randomWeapons.Length);
                                chestLoot = randomWeapons[weaponIndex]; // here weapon
								break;
						}

                        SpawnLoot();
						break;
				}  
            }
        }
        private void SpawnRat() {
            Random random = new Random();
            //create random amount 0 to 2
            int percentage = random.Next(0, 101);
			float direction = 1;
			if (percentage <= 70) return;
            if(percentage <= 90)
            {
				direction *= -1;
				Rat rat = InstanceService.Instantiate(new Rat());
				rat.SetPosition(GetPositionX() + 4 * direction, GetPositionY());
                return;
			}

            // percent is between 90 and 100
			for (int i = 0; i < 2; i++)
			{
				direction *= -1;
				Rat rat = InstanceService.Instantiate(new Rat());
				rat.SetPosition(GetPositionX() + 4 * direction, GetPositionY());
			}
		}

        public void HealthZero()
        {
			if (isDestroyed) return;

			float pitch = MathUtils.RandomFloatInRange(.8f, 1.1f);
			breakingSound.PlayOneShot(breakingSound.Volume, pitch);

			isDestroyed = true;
			
			collider.Destroy();

			SpawnLoot();
            SpawnRat();

		}
    }
}
