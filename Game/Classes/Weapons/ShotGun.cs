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
using System.Xml.Linq;

namespace JailBreaker.Game.Classes.Weapons
{
    public class ShotGun : GunBase
    {

        private CameraShakeArguments shootShake = new CameraShakeArguments(5, 50, .5f, 0, .2f);

        private bool shouldSpawnBullet = false;

        private GameObject user;
        public override void Construct()
        {
            base.Construct();

            Damage = 5;
            FireRate = 2f;
            durability = 8;
            bulletSpreadAngle = 15f;
            type = WeaponType.Shotgun;
            weaponPos = new Vector2(18, 0);
            muzzlePos = new Vector2(9, -4);
            lastFireTime = FireRate;    
            Sprite shootingSprite = new Sprite("Game/Assets/Textures/Weapons/LaserShotGun.png", 6);
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/ShotGunShot.mp3", false);

            SpriteAnimation shooting = new SpriteAnimation(shootingSprite, 2f, false, false);
            shooting.onAnimationFinished += () =>
            {
                shouldSpawnBullet = true;
                shooting.SetFrame(1);
            };

            AnimationControllerState shootingstate = new AnimationControllerState("Shooting", shooting);

            AnimationControllerState[] allstates =
            {
                shootingstate,
            };

            AnimationController controller = new AnimationController(allstates);


            renderer = new SpriteComponent(this, shootingSprite, Raylib_cs.Color.White, "PlayerShotGun");
            renderer.SortingLayer = SortingLayers.Player;
            renderer.ZIndex = 1;

            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();
            Name = "ShotGun";
        }

        public override void Update()
        {
            base.Update();
            if (renderer.FlipSpriteHorizontaly)
            {
                muzzlePos = new Vector2(9, 3);
            }
            else
            {
                muzzlePos = new Vector2(9, -4);
            }
            if (shouldSpawnBullet)
            {
                shouldSpawnBullet = false;
                Vector2 dir = MathUtils.RotateVector2InDeg(GetForwardVector(), MathUtils.RandomFloatInRange(-bulletSpreadAngle, bulletSpreadAngle));
                for (int i = 0; i < 8; i++)
                {
                    CameraService.StartCameraShake(shootShake);
                    InstanceService.Instantiate(new Bullet(500.0f, dir, GetPosition() + GetForwardVector() * muzzlePos.X + GetUpVector() * muzzlePos.Y, Damage, 1, user));
                    dir = MathUtils.RotateVector2InDeg(GetForwardVector(), MathUtils.RandomFloatInRange(-bulletSpreadAngle, bulletSpreadAngle));
                }
                attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
                foreach (InstantiableComponent component in user.Components)
                {
                    if (component is PhysicsComponent p)
                    {
                        p.AddForce(-dir, 70.0f, 1000.0f);
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
            user = origin;
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new ShotGunThrowable(300.0f, this.GetForwardVector(), GetPosition(), 5, 1, origin));
        }
    }
}
