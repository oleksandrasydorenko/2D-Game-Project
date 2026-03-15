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
    public class Pistol : GunBase
    {

        private CameraShakeArguments shootShake = new CameraShakeArguments(2, 50, .5f, 0, .2f);

        private bool shouldSpawnBullet = false;

        private GameObject user;
        public override void Construct()
        {
            base.Construct();

            Damage = 10;
            FireRate = 0.3f;
            durability = 15;
            type = WeaponType.Pistol;
            weaponPos = new Vector2(15, -1);
            muzzlePos = new Vector2(7, -2);
            Sprite shootingSprite = new Sprite("Game/Assets/Textures/Weapons/LaserPistol.png", 5);
            AnimationSignal spawnBullet = new AnimationSignal(5);
            AnimationSignal[] signals = { spawnBullet };
            
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/PistolShot.mp3", false);
            

            SpriteAnimation shooting = new SpriteAnimation(shootingSprite, 1f, false, false, signals);

            
            AnimationControllerState shootingstate = new AnimationControllerState("Shooting", shooting);

            AnimationControllerState[] allstates =
            {
                shootingstate,
            };

            AnimationController controller = new AnimationController(allstates);


            renderer = new SpriteComponent(this, shootingSprite, Raylib_cs.Color.White, "PlayerPistol");
            renderer.SortingLayer = SortingLayers.Player;
            renderer.ZIndex = 1;

            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();
            Name = "Pistol";

            spawnBullet.onAnimationSignalTriggered += () => {
                shouldSpawnBullet = true;
                shooting.SetFrame(1);
            };
        }

        public override void Update()
        {
            base.Update();
            if (renderer.FlipSpriteHorizontaly)
            {
                muzzlePos = new Vector2(7, 2);
            }
            else
            {
                muzzlePos = new Vector2(7, -2);
            }
            if (shouldSpawnBullet)
            {
                shouldSpawnBullet = false;
                CameraService.StartCameraShake(shootShake);
                InstanceService.Instantiate(new Bullet(500.0f, this.GetForwardVector(), GetPosition() + GetForwardVector() * muzzlePos.X + GetUpVector() * muzzlePos.Y, Damage, 1, user));
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
            attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
            user = origin;
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new PistolThrowable(300.0f, this.GetForwardVector(), GetPosition() , 5, 1, origin));
        }
    }
}
