using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using JailBreaker.Game.Classes.Weapons;
using Raylib_cs;
using RocketEngine;

namespace JailBreaker.Weapons
{
    
    public abstract class WeaponBase : GameObject
    {
        public int Damage { get; set; } = 1;
        
        public bool IsAutomatic { get; set; } = false;
        public SpriteComponent renderer;
        public WeaponType type;
        public Vector2 weaponPos;
        public int durability = 0;
        protected AudioComponent attackSound;
        protected AudioComponent throwSound;
        protected AudioComponent dryShotSound;

        public override void Construct()
        {
            base.Construct();
            throwSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/ThrowWeapon.mp3", false);
            dryShotSound = new AudioComponent(this, "Game/Assets/Audio/Weapons/DryShot.mp3");
        }

        public abstract bool Shoot(GameObject origin);
        

        

        public virtual void Throw(GameObject origin)
        {
           throwSound.PlayOneShot();
        }
    }
}
