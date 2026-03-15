using RocketEngine;
using RocketEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{
    public class Fists : MeeleBase
    {
        public override void Construct()
        {
            base.Construct();
            Damage = 10;
            attackSpeed = 0.3f;
            attackRange = 35f;
            Name = "Fists";
            type = WeaponType.Fists;
            weaponPos = new System.Numerics.Vector2(10, 0);
            renderer = new SpriteComponent(this, new Sprite("Game/Assets/Textures/Weapons/FistIcon.png"), Raylib_cs.Color.White, "Fists");
            renderer.visible = false;
            attackSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/Punch.mp3", false);
        }

        public override void Update()
        {
            base.Update();
            lastAttackTime += Time.DeltaTime;
        }

        public override bool Shoot(GameObject origin)
        {
            if (lastAttackTime <= attackSpeed) return false;
            lastAttackTime = 0.0f;
            attackSound.PlayOneShot(volume: attackSound.Volume, pitch: MathUtils.RandomFloatInRange(0.9f, 1.1f));
            InstanceService.Instantiate(new MeeleAttack(attackRange, Damage, origin, GetPosition()));
            return true;
        }
    }
}
