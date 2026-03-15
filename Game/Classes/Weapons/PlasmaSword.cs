using JailBreaker.Game.Classes.Weapons.Projectiles;
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
    public class PlasmaSword : MeeleBase
    {
        public override void Construct()
        {
            base.Construct();
            Damage = 30;
            attackSpeed = 1f;
            attackRange = 50f;
            durability = 10;
            Name = "PlasmaSword";
            type = WeaponType.PlasmaSword;
            weaponPos = new Vector2(37, 1);

            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/Sword.mp3");

            Sprite weaponSprite = new Sprite("Game/Assets/Textures/Weapons/PlasmaSword.png",8);
            renderer = new SpriteComponent(this,weaponSprite, Raylib_cs.Color.White, "PlasmaSword");
            SpriteAnimation attack = new SpriteAnimation(weaponSprite, 0.7f, false, false);
            AnimationControllerState attackState = new AnimationControllerState("Attacking", attack);
            AnimationController controller = new AnimationController(attackState);
            animator = new SpriteAnimatorComponent(this, renderer, controller);
            attack.onAnimationFinished += () =>
            {
                attack.SetFrame(1);
            };
            animator.PauseAnimator();
        }

        public override void Update()
        {
            base.Update();
            lastAttackTime += Time.DeltaTime;
        }

        public override bool Shoot(GameObject origin)
        {
            if (durability <= 0)
            {
                dryShotSound.PlayOneShot(pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
                return false;
            }
            if (lastAttackTime <= attackSpeed) return false;
            durability--;
            lastAttackTime = 0.0f;
            animator.PauseAnimator(false);
            animator.SetState("Attacking");
            attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
            InstanceService.Instantiate(new MeeleAttack(attackRange, Damage, origin, GetPosition()));
            return true;
        }

        public override void Throw(GameObject origin)
        {
            base.Throw(origin);
            InstanceService.Instantiate(new SwordThrowable(300.0f, this.GetForwardVector(), GetPosition(), 5, 1, origin));
        }
    }
}
