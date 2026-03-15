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
    public class MachineGun : GunBase
    {
        private AudioComponent shootSound;

        private CameraShakeArguments shootShake = new CameraShakeArguments(2, 50, .5f, 0, .2f);

        public override void Construct()
        {
            base.Construct();

            Damage = 4;
            FireRate = 0.15f;
            IsAutomatic = true;
            durability = 40;
            bulletSpreadAngle = 5f;
            type = WeaponType.MachineGun;
            weaponPos = new Vector2(12, 0);
            muzzlePos = new Vector2(9, -4);
            Sprite shootingSprite = new Sprite("Game/Assets/Textures/Weapons/LaserMachineGun.png", 4);
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/MachineGun.mp3", false);

            SpriteAnimation shooting = new SpriteAnimation(shootingSprite, 2f, false, false);


            AnimationControllerState shootingstate = new AnimationControllerState("Shooting", shooting);

            AnimationControllerState[] allstates =
            {
                shootingstate,
            };

            AnimationController controller = new AnimationController(allstates);


            renderer = new SpriteComponent(this, shootingSprite, Raylib_cs.Color.White, "PlayerMachineGun");
            renderer.SortingLayer = SortingLayers.Player;
            renderer.ZIndex = 1;

            animator = new SpriteAnimatorComponent(this, renderer, controller);
            animator.PauseAnimator();
            Name = "Machine Gun";
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
            var dir = MathUtils.RotateVector2InDeg(GetForwardVector(),MathUtils.RandomFloatInRange(-bulletSpreadAngle,bulletSpreadAngle));
            
            attackSound.PlayOneShot(volume:attackSound.Volume,pitch: MathUtils.RandomFloatInRange(0.9f,1.1f));
            CameraService.StartCameraShake(shootShake);
            InstanceService.Instantiate(new Bullet(400.0f, dir, GetPosition() + GetForwardVector() * muzzlePos.X + GetUpVector() * muzzlePos.Y, Damage, 1, origin));
            foreach (InstantiableComponent component in origin.Components)
            {
                if (component is PhysicsComponent p)
                    p.AddForce(-dir, 20.0f, 100.0f);
            }
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new MachineGunThrowable(300.0f, this.GetForwardVector(), GetPosition(), 20, 1, origin));
        }
    }
}
