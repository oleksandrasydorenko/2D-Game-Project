using JailBreaker.Game.Classes.Weapons.Projectiles;
using JailBreaker.Player;
using JailBreaker.Weapons;
using RocketEngine;
using RocketEngine.Scenemanagement;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{
    public class PlasmaLauncher : GunBase
    {
        private AudioComponent shootSound;

        private CameraShakeArguments shootShake = new CameraShakeArguments(10, 50, .5f, 0, .2f);

        private bool shouldSpawnBullet = false;

        private GameObject user;
        public override void Construct()
        {
            base.Construct();

            Damage = 10;
            FireRate = 0.5f;
            durability = 5;
            type = WeaponType.PlasmaLauncher;
            weaponPos = new Vector2(4, -3);
            muzzlePos = new Vector2(9, -4);
            bulletPosOffsetX = 10;

            Sprite shootingSprite = new Sprite("Game/Assets/Textures/Weapons/PlasmaLauncher.png", 11);
            AnimationSignal spawnBullet = new AnimationSignal(11);
            AnimationSignal[] signals = { spawnBullet };
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/SniperShoot.mp3", false);
            shootSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/WeaponLoad.mp3");
            SpriteAnimation shooting = new SpriteAnimation(shootingSprite, 1f, false, animationSignals: signals);
            AnimationControllerState shootingstate = new AnimationControllerState("Shooting", shooting);
            AnimationController controller = new AnimationController(shootingstate);

            renderer = new SpriteComponent(this, shootingSprite, Raylib_cs.Color.White, "PlasmaLauncher");
            renderer.SortingLayer = SortingLayers.Player;
            renderer.ZIndex = 1;

            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();

            Name = "PlasmaLauncher";

            spawnBullet.onAnimationSignalTriggered += () => {
                shouldSpawnBullet = true;
                shooting.SetFrame(1);
            };

            BoxCollider2D testcollider = new BoxCollider2D(this, 0, 0, shootingSprite.FrameWidth * renderer.SpriteScale, shootingSprite.FrameHeight * renderer.SpriteScale);
            
        }

        public override void Start()
        {
                       base.Start();
           
        }
        public override void Update()
        {
            base.Update();
            if (renderer.FlipSpriteHorizontaly)
            {
                muzzlePos = new Vector2(9, 2);
            }
            else
            {
                muzzlePos = new Vector2(9, 0);
            }
            if (shouldSpawnBullet)
            {
                shouldSpawnBullet = false;
                var dir = Vector2.Normalize(GetForwardVector());
                CameraService.StartCameraShake(shootShake);
                attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
                InstanceService.Instantiate(new PlasmaBall(400.0f, dir, GetPosition() + GetForwardVector() * muzzlePos.X + GetUpVector() * muzzlePos.Y, Damage, 1, user));
                foreach (InstantiableComponent component in user.Components)
                {
                    if (component is PhysicsComponent p)
                    {
                        p.AddForce(-dir, 130.0f, 1000.0f);
                        break;
                    }

                }
            }
        }

        public override bool Shoot(GameObject origin)
        {
            if (lastFireTime < FireRate) return false;
            if (durability <= 0)
            {
                dryShotSound.PlayOneShot(pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
                return false;
            }

            durability -= 1;
            lastFireTime = 0;
            animator.PauseAnimator(false);
            animator.SetState("Shooting");
            shootSound.PlayOneShot(volume: attackSound.Volume, pitch: 1.5f);
            user = origin;
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new PlasmaLauncherThrowable(300.0f, this.GetForwardVector(), GetPosition(), 5, 1, origin));
        }
    }
}
