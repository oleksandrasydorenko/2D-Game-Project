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
    public abstract class GunBase : WeaponBase
    {
        public SpriteAnimatorComponent animator;
        public float FireRate { get; set; } = 1.0f; // Schüsse pro Sekunde
        protected float lastFireTime = 0.0f;
        protected float bulletPosOffsetX = 1;
        protected float bulletSpreadAngle = 2.0f;
        private Vector2 direction;
        protected Vector2 muzzlePos;
        public Sprite projectileSprite;

        public override void Update()
        {
            lastFireTime += Time.DeltaTime;
        }
    }
}
