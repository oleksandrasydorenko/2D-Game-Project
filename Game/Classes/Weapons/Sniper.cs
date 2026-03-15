using JailBreaker.Game.Classes.Weapons.Projectiles;
using JailBreaker.Weapons;
using RocketEngine;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{
    public class Sniper : GunBase
    {
        private AudioComponent shootSound;

        private CameraShakeArguments shootShake = new CameraShakeArguments(10, 50, .5f, 0, .2f);
        public override void Construct()
        {
            base.Construct();

            Damage = 40;
            FireRate = 2f;
            durability = 4;
            bulletSpreadAngle = 0f;
            type = WeaponType.Sniper;
            weaponPos = new Vector2(20, -1);
            muzzlePos = new Vector2(14, -4);
            lastFireTime = FireRate;
            Sprite shootingSprite = new Sprite("Game/Assets/Textures/Weapons/LaserSniper.png", 3);
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/SniperShoot.mp3", false);

            SpriteAnimation shooting = new SpriteAnimation(shootingSprite, 0.5f, false, false);
            AnimationControllerState shootingstate = new AnimationControllerState("Shooting", shooting);

            AnimationControllerState[] allstates =
            {
                shootingstate,
            };

            AnimationController controller = new AnimationController(allstates);


            renderer = new SpriteComponent(this, shootingSprite, Raylib_cs.Color.White, "PlayerSniper");
            renderer.SortingLayer = SortingLayers.Player;
            renderer.ZIndex = 1;

            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();
            Name = "Sniper";
        }

        public override void Update()
        {
            base.Update();
            if (renderer.FlipSpriteHorizontaly)
            {
                muzzlePos = new Vector2(14, 2);
            }
            else
            {
                muzzlePos = new Vector2(14, -2);
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
            Vector2 dir = MathUtils.RotateVector2InDeg(GetForwardVector(), MathUtils.RandomFloatInRange(-bulletSpreadAngle, bulletSpreadAngle));
            CameraService.StartCameraShake(shootShake);
            attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
            InstanceService.Instantiate(new SniperBullet(700.0f, dir, GetPosition() + GetForwardVector() * muzzlePos.X + GetUpVector() * muzzlePos.Y, Damage, 5, origin));
            foreach (InstantiableComponent component in origin.Components)
            {
                if (component is PhysicsComponent p)
                {
                    p.AddForce(-dir, 200.0f, 1000.0f);
                    break;
                }

            }
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new SniperThrowable(300.0f, this.GetForwardVector(), GetPosition(), 5, 1, origin));
        }
    }
}
