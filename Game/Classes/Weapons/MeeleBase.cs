using JailBreaker.Weapons;
using RocketEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailBreaker.Game.Classes.Weapons
{
    public abstract class MeeleBase : WeaponBase
    {
        protected float attackSpeed = 1.0f; // Angriffe pro Sekunde
        protected float lastAttackTime = 0.0f;
        protected float attackRange = 1.5f; // Reichweite des Angriffs
        protected SpriteAnimatorComponent animator;
    }
}
